namespace Util;

using GameEngine;

/**
 * Author:      Joao Paulo B Faria
 * Date:        Oct/2022
 * Description: Class utility to create a bitmap with the current system path
 */
public class BitmapEx
{
    //no public constructor
    private BitmapEx() 
    {}

    //method factory
    public static Bitmap New(string filepath) 
    {
        return new Bitmap(@"" + Launcher.path + filepath);
    }

    public const int SRCCOPY = 0xcc0020; // we want to copy an in memory image

    [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]        
    public static extern int BitBlt(IntPtr hdcDest,     // handle to destination DC (device context)          
                                     int nXDest,         // x-coord of destination upper-left corner          
                                     int nYDest,         // y-coord of destination upper-left corner          
                                     int nWidth,         // width of destination rectangle          
                                     int nHeight,        // height of destination rectangle          
                                     IntPtr hdcSrc,      // handle to source DC          
                                     int nXSrc,          // x-coordinate of source upper-left corner          
                                     int nYSrc,          // y-coordinate of source upper-left corner
                                     System.Int32 dwRop  // raster operation code          
                                     );
}