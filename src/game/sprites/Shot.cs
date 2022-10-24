namespace Game;

using Util;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Represents player shot
 */
public class Shot : GameSprite
{
    private IGame GameRef;
    private const byte AVAILABLE        = 0;
    private const byte UNAVAILABLE      = 1;
    private int ShotStatus              = AVAILABLE;
    private bool IsBulletDestroyed      = true;
    private bool StartDelay             = false;
    private int Delay                   = 1_200_000;
    private long Framecounter           = 0;

    /**
     * Constructor
     */
    public Shot(IGame game, int width, int height, int X, int Y, int velocity) : base(width, height, X, Y, velocity, 0)
    {
        this.GameRef = game;
        this.SpriteImage = LoadingStuffs.GetInstance().GetImage("shot");
    }

    public void TriggerShot(float airplaneX, float airplaneY, float airplaneWidth) 
    {
        this.IsBulletDestroyed = false;
        this.X = airplaneX + (airplaneWidth/2);
        this.Y = airplaneY - this.Height;
    }

    public void UpdateShotToHitOrLoose() 
    {
        this.IsBulletDestroyed = true;
        this.ShotStatus = AVAILABLE;
    }

    /**
     * control & update each shot
     */
    public override void Update(long frametime, bool colliding = false)
    {
        if (!this.IsBulletDestroyed) 
        {
            float step = (float)(this.Velocity * ((double)frametime / 10_000_000));
            this.Y -= step;
            this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
            this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);       

            if (this.Y + 20 < 0) 
            {
                this.UpdateShotToHitOrLoose();
            } 
            else 
            {
                foreach (var item in this.GameRef.GetCurrentScreenSprites()) 
                {
                    if (this.CollisionDetection(item) && !item.Destroyed) 
                    {
                        this.IsBulletDestroyed  = true;
                        this.StartDelay         = true;
                        item.SetCollision(false);
                        break;
                    }
                }
            }
        }

        if (this.StartDelay) {
            this.Framecounter += frametime;
            if (this.Framecounter > this.Delay) 
            {
                this.StartDelay = false;
                this.Framecounter = 0;
                this.ShotStatus = AVAILABLE;
            }
        }
    }

    public override void Draw(Graphics gfx) 
    {
        if (this.SpriteImage != null)
        {
            if (!this.IsBulletDestroyed)
            {
                gfx.DrawImage(this.SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
            }
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