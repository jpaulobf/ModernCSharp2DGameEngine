namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: Class representing the Main Player (Player Sprite)
 */
public class PlayerSprite : GameSprite 
{
    private IGame GameRef;
    private Bitmap LocalSpriteImage;
    private Bitmap SpriteSplosion;
    private GameSprite Shot;
    private const string SplosionImageFilePath  = "img\\sprite_splosion.png";
    private const string ShotImageFilePath      = "img\\shot_sprite.png";
    private float OgWidth                       = 0;
    private float OgHeight                      = 0;
    private int OgX                             = 0;
    private int OgY                             = 0;
    private SolidBrush YellowBrush              = new SolidBrush(Color.FromArgb(255, 232, 232, 74));
    public bool Colliding { get; set; }         = false;
    private System.Media.SoundPlayer player     = new System.Media.SoundPlayer();

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(IGame game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.GameRef            = game;
        this.SpriteSplosion     = new Bitmap(@"img\\sprite_splosion.png");
        this.LocalSpriteImage   = new Bitmap(@imageFilePath);
        this.Shot               = new Shot(game, ShotImageFilePath, 5, 18, 0, 0, 600);
        this.OgWidth            = width;
        this.OgHeight           = height;
        this.OgX                = X;
        this.OgY                = Y;

        this.player.SoundLocation = "sfx\\shot.wav";
    }

    /**
     * Shot
     */
    public void Shooting() 
    {
        Shot shot = ((Shot)this.Shot);
        if (shot.IsShotAvailable()) {
            player.Play();
            shot.TriggerShot(this.X, this.Y, this.Width);
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
     * Player Sprite update method
     */
    public override void Update(long timeframe, bool colliding = false) {
        if (!this.Colliding) 
        {
            if (!this.Lefting && !this.Righting) 
            {
                this.SourceStartX = (short)Width; //Default
            } 
            else if (this.Lefting) 
            {
                this.SourceStartX = 0;
            } 
            else if (this.Righting) 
            { 
                this.SourceStartX = (short)(Width * 2);
            }
        } 
        else 
        {
            this.SourceStartX = 0;
            this.Width = 32;
            this.Height = 25;
            this.SpriteImage = this.SpriteSplosion;
        }

        //update the shot object
        this.Shot.Update(timeframe);

        //define the source & destine rect
        this.SourceRect = new Rectangle(SourceStartX, SourceStartY, (short)Width - 1, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }

    /**
     * Draw Player Sprite
     */
    public override void Draw(Graphics gfx)
    {
        //call base draw
        base.Draw(gfx);

        //draw the shot object
        this.Shot.Draw(gfx);
    }

    /**
     * Reset player sprite
     */
    public override void Reset()
    {
        this.SpriteImage    = this.LocalSpriteImage;
        this.Colliding      = false;
        this.Width          = this.OgWidth;
        this.Height         = this.OgHeight;
        this.X              = this.OgX;
        this.Y              = this.OgY;
    }

    public override void SetCollision(bool isPlayerCollision)
    {
    }
}