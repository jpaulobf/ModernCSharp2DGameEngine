namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Represents player shot
 */
public class Shot : GameSprite
{
    private IGame GameRef;
    private const byte AVAILABLE = 0;
    private const byte UNAVAILABLE = 1;
    private int ShotStatus = AVAILABLE;
    private bool HasShot = false;

    /**
     * Constructor
     */
    public Shot(IGame game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity)
    {
        this.GameRef = game;
    }

    public void TriggerShot(float airplaneX, float airplaneY, float airplaneWidth) 
    {
        this.HasShot = true;
        this.X = airplaneX + (airplaneWidth/2);
        this.Y = airplaneY - this.Height;
    }

    public void UpdateShotToHitOrLoose() 
    {
        this.HasShot = false;
        this.ShotStatus = AVAILABLE;
    }

    /**
     * control & update each shot
     */
    public override void Update(long frametime, bool colliding = false)
    {
        if (this.HasShot) 
        {
            float step = (float)(this.Velocity * ((double)frametime / 10_000_000));
            this.Y -= step;
            this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
            this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);       
            
            if (this.Y < 0) {
                this.UpdateShotToHitOrLoose();
            } 
            else 
            {
                foreach (var item in this.GameRef.GetCurrentScreenSprites()) 
                {
                    GameSprite gamesprite = item.GetGameSprite();
                    if (this.CollisionDetection(gamesprite) && !gamesprite.Destroyed) 
                    {
                        this.UpdateShotToHitOrLoose();
                        gamesprite.SetCollision(false);
                        break;
                    }
                }
            }
        }
    }

    public override void Draw(Graphics gfx) 
    {
        if (this.HasShot)
        {
            gfx.DrawImage(this.SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
        }
    }

    public bool IsShotAvailable() 
    {
        return (this.ShotStatus == AVAILABLE);
    }

    public void DisableShot() 
    {
        this.ShotStatus = UNAVAILABLE;
    }
    
    /**
     * reset
     */
    public override void Reset()
    {

    }

    public override void SetCollision(bool isPlayerCollision)
    {
        
    }
}