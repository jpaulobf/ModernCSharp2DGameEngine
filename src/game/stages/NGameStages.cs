namespace Game.Stages;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Definition: This class represent the Game Stages, with their background and enemy/static sprites.
 */
public class NGameStages : IStagesDef 
{
    private IGame GameRef;
    private float ScaleW                                = 1.0F;
    private float ScaleH                                = 1.0F;
    private const short PIXEL_WIDTH                     = 18;
    private const byte PIXEL_HEIGHT                     = 4;
    private SolidBrush [] Brushes                       = new SolidBrush[] {new SolidBrush(Color.FromArgb(255, 0, 0, 0)),       //black
                                                                            new SolidBrush(Color.FromArgb(255, 110, 156, 66)),  //green
                                                                            new SolidBrush(Color.FromArgb(255, 53, 95, 24)),    //dark green
                                                                            new SolidBrush(Color.FromArgb(0, 0, 0, 0)),         //transparent black
                                                                            new SolidBrush(Color.FromArgb(255, 255, 255, 255)), //white
                                                                            new SolidBrush(Color.FromArgb(255, 111, 111, 111)), //silver
                                                                            new SolidBrush(Color.FromArgb(255, 170, 170, 170)), //dark gray
                                                                            new SolidBrush(Color.FromArgb(255, 234, 234, 70)),  //yellow
                                                                            new SolidBrush(Color.FromArgb(255, 45, 50, 184))};  //dark blue
    private RectangleF DrawRect                          = new RectangleF(0f, 0f, PIXEL_WIDTH, PIXEL_HEIGHT);
    protected volatile short CurrentLine                = 574;
    private const short SCREEN_LINES                    = 107 + 1; //one extra buffer line
    private const byte OPENING_LINES                    = 108;
    protected volatile short CurrentOpeningLine         = 0;
    private const byte STAGE_OFFSET                     = 1;
    private short CURRENT_STAGE                         = 1 - STAGE_OFFSET;
    private short CURRENT_STAGE_LINES_DIFF              = 0;
    private volatile float Offset                       = 0;
    private volatile float OpeningOffset                = 0;
    private volatile short TransitionOffset             = 0;
    private volatile int StartScreenFrame               = 0;
    private volatile int EndScreenFrame                 = 0;
    private volatile int CurrentLineYPosition           = 0;
    private volatile bool CanDrawBackground             = false;
    private volatile bool CanDrawStageOpening           = true;
    private volatile bool CanStartTheStage              = false;
    private volatile bool CanStartStageOpening          = true;
    private volatile bool RunStage                      = false;
    private volatile bool TransitionBtwStages           = false;
    private List<GameSprite> CurrentStageSprites;
    //private List<GameSprite> NextStageSprites;
    private Dictionary<int, GameSprite> CurrentStageSpritesDefition = new Dictionary<int, GameSprite>();
    private Dictionary<int, GameSprite> NextStageSpritesDefition    = new Dictionary<int, GameSprite>();
    
}
