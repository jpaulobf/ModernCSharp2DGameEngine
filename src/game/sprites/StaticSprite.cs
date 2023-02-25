using Util;

namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: Class representing static sprites (like houses & fuel)
 */
public class StaticSprite : GameSprite
{
    private IGame GameRef;
    private Bitmap OGSpriteImage;
    private Bitmap Explosion1               = LoadingStuffs.GetInstance().GetImage("heli-explosion-1");
    private Bitmap Explosion2               = LoadingStuffs.GetInstance().GetImage("heli-explosion-2");

    /**
     * Static Sprite constructor
     */
    public StaticSprite(IGame game, int width, int height, byte type, int X = 0, int Y = 0) : base(width, height, X, Y, 0, type) {
        this.GameRef = game;
        switch(type)
        {
            case FUEL:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("fuel");
                this.OGSpriteImage  = LoadingStuffs.GetInstance().GetImage("fuel");
            break;
            case HOUSE:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("house-1");
                this.OGSpriteImage  = LoadingStuffs.GetInstance().GetImage("house-1");
            break;
            case HOUSE2:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("house-2");
                this.OGSpriteImage  = LoadingStuffs.GetInstance().GetImage("house-2");
            break;
            case BRIDGE:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("bridge");
                this.OGSpriteImage  = LoadingStuffs.GetInstance().GetImage("bridge");
            break;
            default:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("pixel");
                this.OGSpriteImage  = LoadingStuffs.GetInstance().GetImage("pixel");
            break;
        }
    }

    /**
     * update
     */
    public override void Update(long frametime, bool colliding = false) {
            
        this.SourceRect = new RectangleF(0, 0, this.Width, this.Height);
        this.DestineRect = new RectangleF(this.X, this.Y, this.Width, this.Height);
        int bridgeExplosionX = 350;

        if (!this.Destroyed && this.Type == FUEL) 
        {
            //TODO: MELHORAR......
            if (this.CollisionDetection(this.GameRef.GetPlayer().GetPlayerSprite())) 
            {
                this.GameRef.GetPlayer().AddFuel(frametime);
            }
            else
            {
                this.GameRef.GetPlayer().Refueling = false;
            }
        }

        //this will start after collision
        if (this.AnimateExplosion) {
            this.AnimationCounter += frametime;

            //this will start after explosion start
            if (this.AnimationCounter > 1_000_000 && this.AnimationCounter < 4_000_000) 
            {
                this.SpriteImage = this.Explosion1;
                if (this.Type == BRIDGE)
                {
                    this.X = bridgeExplosionX;
                }
            } 
            else if (this.AnimationCounter >= 4_000_000 && this.AnimationCounter < 8_000_000) 
            {
                this.SpriteImage = this.Explosion2;
            } 
            else if (this.AnimationCounter >= 8_000_000) 
            {
                this.SpriteImage = this.Pixel;
                this.AnimateExplosion = false;
                this.AnimationCounter = 0;
                this.Destroyed = true;
            }
        }
    }
    
    /**
     * Reset the static sprite
     */
    public override void Reset()
    {
        this.SpriteImage        = this.OGSpriteImage;
        this.TilesNumber        = 0;
        this.Destroyed          = false;
        this.AnimateExplosion   = false;
        this.Destroyed          = false;
        this.X                  = this.OgX;
    }

    public override void SetCollision(bool isPlayerCollision)
    {
        this.TilesNumber = 0;
        this.Destroyed = true;
        this.AnimateExplosion = true;
    }
}