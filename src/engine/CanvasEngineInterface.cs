namespace engine;

public interface CanvasEngineInterface {
    /**
     * Update the game logic / receives the frametime
     * @param frametime
     */
    public void Update(long frametime);

    /**
     * Draw the game / receives the frametime
     * @param frametime
     */
    public void Draw(long frametime);

    /**
     * Render the backbuffer in the canvas
     */
    public void Render();
    
    /**
     * Dispose the graphics
     */
    void GraphicDispose();
}