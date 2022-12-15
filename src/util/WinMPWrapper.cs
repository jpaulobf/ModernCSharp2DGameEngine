using System.Runtime.InteropServices;

namespace MediaPlayer;

/**
 */
public class WinMPWrapper
{
    [DllImport("WMPLib.dll", SetLastError = true)]
    internal static extern int sndPlaySound(IntPtr buffer, int dwFlags);
    //WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();

    
}