namespace game.stages;

public class Conf {
    private int X;
    private int Y;
    private int type;
    private int parameter;
    private bool flag;

    public Conf(int type, int X, int parameter = 1, bool flag = false) {
        this.X = X;
        this.type = type;
        this.parameter = parameter;
        this.flag = flag;
    }
}