namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Class representing the player object
 */
public class Player
{
    private PlayerSprite PlayerSprite;
    private GameSprite Shot;
    private IGame GameRef;
    private Util.SoundPlayerEx ShotSFX          = new Util.SoundPlayerEx(Util.Utility.getCurrentPath() + "sfx\\shot.wav");
    public float AirplaneNoseX                  = 0f;
    public float AirplaneNoseW                  = 0f;
    public bool Colliding { get; set; }         = false;
    public bool NORMAL_SPEED { get; set; }      = true;
    public bool HALF_SPEED { get; set; }        = false;
    public bool DOUBLE_SPEED { get; set; }      = false;
    public bool Flying { get; set; }            = false;
    public bool Refueling { get; set; }         = false;
    public int Velocity { get; set; }
    public bool Lefting { get; set; }
    public bool Righting { get; set; }
    private const byte FUEL_COMPLETE            = 100;
    public float FuelCounter { get; set; }      = FUEL_COMPLETE;
    private const int HALF_FUEL_SPENT           = 1;
    private const int NORMAL_FUEL_SPENT         = 2;
    private const int DOUBLE_FUEL_SPENT         = 4;
    private int CurrentFuelSpent                = NORMAL_FUEL_SPENT;
    protected long FrameCounter                 = 0;
    private const byte DefaultLives             = 5;
    public byte Lives {get;set;}                = DefaultLives;

    /**
     * Class constructor
     */
    public Player(IGame gameRef)
    {
        this.GameRef                = gameRef;
        this.Velocity               = 100;
        this.PlayerSprite           = new PlayerSprite(gameRef, this, 32, 32, 350, 387, this.Velocity);
        this.Shot                   = new Shot(gameRef, 5, 18, 0, 0);
    }

    private void PlayShotSound()
    {
        Task.Run(() =>
            {
                //ShotSFX.PlayAsync();
            }
        );
    }

    /**
     * Shot
     */
    public void Shooting() 
    {
        Shot shot = ((Shot)this.Shot);
        if (shot.IsShotAvailable()) {
            this.PlayShotSound();
            shot.TriggerShot(this.PlayerSprite.X, this.PlayerSprite.Y, this.PlayerSprite.Width);
            shot.DisableShot();
        }
    }

    /**
     * Adding fuel
     */
    public void AddFuel(long frametime)
    {
        this.Refueling = true;
        this.FuelCounter += (float)(((double)frametime * 0.000002)); //(float)(((double)frametime / 10_000_000) * 20);
        if (this.FuelCounter > FUEL_COMPLETE)
        {
            this.FuelCounter = FUEL_COMPLETE;
        }
    }

    /**
     * Draw Method
     */
    internal void Draw(Graphics internalGraphics)
    {
        //draw the player sprite
        this.PlayerSprite.Draw(internalGraphics);

        //draw the shot object
        this.Shot.Draw(internalGraphics);
    }

    /**
     * Update Method
     */
    internal void Update(long frametime)
    {
        if (this.Flying)
        {
            this.FrameCounter += frametime;

            //from time to time, update the fuel counter
            if (this.FrameCounter >= 8_500_000)
            {
                if (!this.Refueling)
                {
                    //this.FuelCounter -= this.CurrentFuelSpent;
                }
                this.FrameCounter = 0;

                //Update the fuel decrease markup
                this.GameRef.UpdateFuelMarker();
                
                //if fuel is empty, then die...
                if (this.FuelCounter <= 0)
                {
                    this.GameRef.PlayerCollided();
                }
            }
        }

        this.PlayerSprite.Update(frametime);

        //update the shot object
        this.Shot.Update(frametime);
    }

    /**
     * Calc movement distance based on frametime
     */
    private float CalcDistance(long frametime) 
    {
        // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
        return ((float)(this.Velocity * ((double)frametime * 0.0000001)));
    }

    /**
     * Control the player to go left
     */
    public void GoLeft(long frametime)      
    {
        this.PlayerSprite.X -= this.CalcDistance(frametime);
        this.Lefting = true;
    }
    
    /**
     * Control the player to go right
     */
    public void GoRight(long frametime) 
    {   
        
        this.PlayerSprite.X += this.CalcDistance(frametime);
        this.Righting = true;
    }

    /**
     * Define the player to go straight
     */
    public void GoStraight() 
    {
        this.Righting = false;
        this.Lefting = false;
        this.NormalSpeed();
    }

    /**
     * Set the current speed to normal speed
     */
    internal void NormalSpeed()
    {
        this.NORMAL_SPEED       = true;
        this.HALF_SPEED         = false;
        this.DOUBLE_SPEED       = false;
        this.CurrentFuelSpent   = NORMAL_FUEL_SPENT;
    }

    /**
     * Set the current speed to half speed
     */
    internal void HalfSpeed()
    {
        this.NORMAL_SPEED       = false;
        this.HALF_SPEED         = true;
        this.DOUBLE_SPEED       = false;
        this.CurrentFuelSpent   = HALF_FUEL_SPENT;
    }

    /**
     * Set the current speed to double speed
     */
    internal void DoubleSpeed()
    {
        this.NORMAL_SPEED       = false;
        this.HALF_SPEED         = false;
        this.DOUBLE_SPEED       = true;
        this.CurrentFuelSpent   = DOUBLE_FUEL_SPENT;
    }

    /**
     * Define as colliding
     */
    public void SetCollision() {
        this.Colliding = true;
    }

    /**
     * Verify if player is still alive
     */
    internal bool PlayerIsAlive()
    {
        return (this.Lives > 0);
    }

    /**
     * Decrease the current player live number
     */
    internal void PlayerDecreaseLive()
    {
        --this.Lives;
    }

    /**
     * Reset player sprite
     */
    public void Reset(bool resetLives = true)
    {
        this.PlayerSprite.Reset();
        this.Colliding          = false;
        this.NORMAL_SPEED       = true;
        this.HALF_SPEED         = false;
        this.DOUBLE_SPEED       = false;
        this.FuelCounter        = FUEL_COMPLETE;
        this.CurrentFuelSpent   = NORMAL_FUEL_SPENT;
        this.Flying             = false;
        if (resetLives)
        {
            this.Lives  = DefaultLives;
        }
    }

    //Accessors
    public GameSprite GetPlayerSprite()     {   return (this.PlayerSprite);             }
    public float GetXPosition()             {   return (this.PlayerSprite.X);           }
    public float GetSpriteWidth()           {   return (this.PlayerSprite.Width);       }
    public void SetXPosition(float x)       {   this.PlayerSprite.X = x;                }
    public float GetYPosition()             {   return (this.PlayerSprite.Y);           }
    public void SetYPosition(float y)       {   this.PlayerSprite.Y = y;                }
    public float GetAirplaneNoseX()         {   return (this.GetXPosition() + 14);      }
    public float GetAirplaneNoseW()         {   return (this.GetAirplaneNoseX() + 6);   }
}