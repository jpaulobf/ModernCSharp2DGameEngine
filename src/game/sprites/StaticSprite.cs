namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: Class representing static sprites (like houses & fuel)
 */
public class StaticSprite : GameSprite
{
    private IGame GameRef;
    private volatile bool AnimateExplosion  = false;
    private long AnimationCounter           = 0;
    private Bitmap OgSprite;
    private Bitmap Explosion1               = new Bitmap(@"img\\heli_explosion_frame1.png");
    private Bitmap Explosion2               = new Bitmap(@"img\\heli_explosion_frame2.png");

    /**
     * Static Sprite constructor
     */
    public StaticSprite(IGame game, string imageFilePath, int width, int height, byte type, int X = 0, int Y = 0) : base(imageFilePath, width, height, X, Y, 0, type) {
        this.GameRef = game;
        this.OgSprite = new Bitmap(@imageFilePath);
    }

    /**
     * update
     */
    public override void Update(long frametime, bool colliding = false) {
            
        this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);

        if (!this.Destroyed) 
        {
            if (this.CollisionDetection(this.GameRef.GetPlayerController().GetPlayerSprite())) 
            {
                Console.WriteLine("Fuel...");
            }
        }

        //this will start after collision
        if (this.AnimateExplosion) {
            this.AnimationCounter += frametime;

            //this will start after explosion start
            if (this.AnimationCounter > 1_000_000 && this.AnimationCounter < 4_000_000) 
            {
                this.SpriteImage = this.Explosion1;
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
        this.SpriteImage = this.OgSprite;
        this.TilesNumber = 0;
        this.Destroyed = false;
        this.AnimateExplosion = false;
        this.Destroyed = false;
    }

    public override void SetCollision(bool isPlayerCollision)
    {
        this.TilesNumber = 0;
        this.Destroyed = true;
        this.AnimateExplosion = true;
    }
}