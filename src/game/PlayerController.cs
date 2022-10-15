using Game;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Class representing the player object
 */
public class PlayerController
{
    private PlayerSprite PlayerSprite;
    private GameSprite Shot;
    private IGame GameRef;
    private System.Media.SoundPlayer player     = new System.Media.SoundPlayer();
    public bool Colliding { get; set; }         = false;
    public bool NORMAL_SPEED { get; set; }      = true;
    public bool HALF_SPEED { get; set; }        = false;
    public bool DOUBLE_SPEED { get; set; }      = false;
    public int Velocity { get; set; }
    public bool Lefting { get; set; }
    public bool Righting { get; set; }

    /**
     * Class constructor
     */
    public PlayerController(IGame gameRef)
    {
        this.GameRef                = gameRef;
        this.Velocity               = 100;
        this.PlayerSprite           = new PlayerSprite(gameRef, this, "img\\airplanetile.png", 32, 32, 350, 387, this.Velocity);
        this.Shot                   = new Shot(gameRef, "img\\shot_sprite.png", 5, 18, 0, 0, 600);
        this.player.SoundLocation   = "sfx\\shot.wav";
    }

    /**
     * Shot
     */
    public void Shooting() 
    {
        Shot shot = ((Shot)this.Shot);
        if (shot.IsShotAvailable()) {
            player.Play();
            shot.TriggerShot(this.PlayerSprite.X, this.PlayerSprite.Y, this.PlayerSprite.Width);
            shot.DisableShot();
        }
    }

    /**
     * Define as colliding
     */
    public void SetCollision() {
        this.Colliding = true;
    }

    /**
     * Set the current speed to normal speed
     */
    internal void NormalSpeed()
    {
        this.NORMAL_SPEED   = true;
        this.HALF_SPEED     = false;
        this.DOUBLE_SPEED   = false;
    }

    /**
     * Set the current speed to half speed
     */
    internal void HalfSpeed()
    {
        this.NORMAL_SPEED   = false;
        this.HALF_SPEED     = true;
        this.DOUBLE_SPEED   = false;
    }

    /**
     * Set the current speed to double speed
     */
    internal void DoubleSpeed()
    {
        this.NORMAL_SPEED   = false;
        this.HALF_SPEED     = false;
        this.DOUBLE_SPEED   = true;
    }

    /**
     * Reset player sprite
     */
    public void Reset()
    {
        this.PlayerSprite.Reset();
        this.Colliding      = false;
        this.NORMAL_SPEED   = true;
        this.HALF_SPEED     = false;
        this.DOUBLE_SPEED   = false;
    }

    /**
     * Draw Method
     */
    internal void Draw(Graphics internalGraphics)
    {
        this.PlayerSprite.Draw(internalGraphics);

        //draw the shot object
        this.Shot.Draw(internalGraphics);
    }

    /**
     * Update Method
     */
    internal void Update(long frametime)
    {
        this.PlayerSprite.Update(frametime);

        //update the shot object
        this.Shot.Update(frametime);
    }

    //Accessors
    public GameSprite GetPlayerSprite()     {   return (this.PlayerSprite);         }
    public float GetXPosition()             {   return (this.PlayerSprite.X);       }
    public float GetSpriteWidth()           {   return (this.PlayerSprite.Width);   }
    public void SetXPosition(int x)         {   this.PlayerSprite.X = x;            }

    public void GoLeft(long frametime)      
    {
        this.PlayerSprite.X -= this.CalcDistance(frametime);
        this.Lefting = true;
    }

    private float CalcDistance(long frametime) 
    {
        // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
        return ((float)(this.Velocity * ((double)frametime / 10_000_000)));
    }
    
    public void GoRight(long frametime) 
    {   
        
        this.PlayerSprite.X += this.CalcDistance(frametime);
        this.Righting = true;
    }

    public void GoStraight() 
    {
        this.Righting = false;
        this.Lefting = false;
        this.NormalSpeed();
    }
}