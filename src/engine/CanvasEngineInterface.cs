namespace engine;

public interface CanvasEngineInterface {
    /**
     * Update the game logic / receives the frametime
     * @param frametime
     */
    public void update(ulong frametime);

    /**
     * Draw the game / receives the frametime
     * @param frametime
     */
    public void draw(ulong frametime);

    public void Render();
    
    void GraphicDispose();
    
    void ReleaseHdc();
}