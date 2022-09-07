namespace engine;

public interface CanvasEngineInterface {
    /**
     * Update the game logic / receives the frametime
     * @param frametime
     */
    public void update(long frametime);

    /**
     * Draw the game / receives the frametime
     * @param frametime
     */
    public void draw(long frametime);
}