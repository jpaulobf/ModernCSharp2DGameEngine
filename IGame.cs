public interface IGame {
    public void Load();

    public void Unload();

    public void Update(TimeSpan gameTime);

    public void Draw(Graphics gfx);
}