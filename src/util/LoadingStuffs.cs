namespace Util;

/**
 *
 */
public class LoadingStuffs
{

    private static LoadingStuffs? Instance      = null;
    private Dictionary<string, Bitmap> Images   = new Dictionary<string, Bitmap>();

    private LoadingStuffs()
    {
        try 
        {
            Bitmap image;
            image = BitmapEx.New("img\\ship_explosion_frame1.png");
            Images.Add("ship-explosion-1", image);

            image = BitmapEx.New("img\\ship_explosion_frame2.png");
            Images.Add("ship-explosion-2", image);

            image = BitmapEx.New("img\\heli_explosion_frame1.png");
            Images.Add("heli-explosion-1", image);

            image = BitmapEx.New("img\\heli_explosion_frame2.png");
            Images.Add("heli-explosion-2", image);

            image = BitmapEx.New("img\\pixel.png");
            Images.Add("pixel", image);

            image = BitmapEx.New("img\\sprite_splosion.png");
            Images.Add("sprite-explosion", image);

            image = BitmapEx.New("img\\house.png");
            Images.Add("house-1", image);

            image = BitmapEx.New("img\\house2.png");
            Images.Add("house-2", image);

            image = BitmapEx.New("img\\fuel.png");
            Images.Add("fuel", image);

            image = BitmapEx.New("img\\ship.png");
            Images.Add("ship", image);

            image = BitmapEx.New("img\\ship_r.png");
            Images.Add("ship-r", image);

            image = BitmapEx.New("img\\helitile.png");
            Images.Add("heli-tile", image);

            image = BitmapEx.New("img\\helitile_r.png");
            Images.Add("heli-tile-r", image);

            image = BitmapEx.New("img\\shot_sprite.png");
            Images.Add("shot", image);

            image = BitmapEx.New("img\\airplanetile.png");
            Images.Add("airplane-tile", image);

            image = BitmapEx.New("img\\fuel_meter_frame.png");
            Images.Add("fuel-frame", image);

            image = BitmapEx.New("img\\fuel_meter.png");
            Images.Add("fuel-meter", image);

            image = BitmapEx.New("img\\n0.png");
            Images.Add("number-0", image);

            image = BitmapEx.New("img\\n1.png");
            Images.Add("number-1", image);

            image = BitmapEx.New("img\\n2.png");
            Images.Add("number-2", image);

            image = BitmapEx.New("img\\n3.png");
            Images.Add("number-3", image);

            image = BitmapEx.New("img\\n4.png");
            Images.Add("number-4", image);

            image = BitmapEx.New("img\\n5.png");
            Images.Add("number-5", image);

            image = BitmapEx.New("img\\n6.png");
            Images.Add("number-6", image);

            image = BitmapEx.New("img\\n7.png");
            Images.Add("number-7", image);
            
            image = BitmapEx.New("img\\n8.png");
            Images.Add("number-8", image);

            image = BitmapEx.New("img\\n9.png");
            Images.Add("number-9", image);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /**
     * Return the stored image
     * @param imageName
     * @return
     */
    public Bitmap GetImage(String imageName) {
        return (this.Images[imageName]);
    }

    /**
     * Recover the singleton instance  
     * @return
     */
    public static LoadingStuffs GetInstance() {
        if (Instance == null) {
            Instance = new LoadingStuffs();
        }
        return Instance;
    }
}