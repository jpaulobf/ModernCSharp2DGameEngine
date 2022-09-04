namespace engine;

public interface IGame {
    public void Load();

    public void Unload();

    public void Update(long frametime);

    public void Draw(Graphics gfx);

    public Size Resolution { get; set; }

    public void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e);

    public void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e);

    public void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e);
}