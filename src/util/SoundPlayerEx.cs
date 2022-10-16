namespace Util;

using System.Runtime.InteropServices;
using System.Media;
using System.Text;

public class SoundPlayerEx : SoundPlayer
{
    public bool Finished { get; private set; }

    private Task _playTask;
    private CancellationTokenSource _tokenSource = new CancellationTokenSource();
    private CancellationToken _ct;
    private string _fileName;
    private bool _playingAsync = false;

    public event EventHandler SoundFinished;

    public SoundPlayerEx(string soundLocation)
        : base(soundLocation)
    {
        _fileName = soundLocation;
        _ct = _tokenSource.Token;
    }

    public void PlayAsync()
    {
        Finished = false;
        _playingAsync = true;
        Task.Run(() =>
        {
            try
            {
                double lenMs = SoundInfo.GetSoundLength(_fileName);
                DateTime stopAt = DateTime.Now.AddMilliseconds(lenMs);
                this.Play();
                while (DateTime.Now < stopAt)
                {
                    _ct.ThrowIfCancellationRequested();
                    //The delay helps reduce processor usage while "spinning"
                    Task.Delay(10).Wait();
                }
            }
            catch (OperationCanceledException)
            {
                base.Stop();
            }
            finally
            {
                OnSoundFinished();
            }

        }, _ct);            
    }

    public new void Stop()
    {
        if (_playingAsync)
            _tokenSource.Cancel();
        else
            base.Stop();   //To stop the SoundPlayer Wave file
    }

    protected virtual void OnSoundFinished()
    {
        Finished = true;
        _playingAsync = false;

        EventHandler handler = SoundFinished;

        if (handler != null)
            handler(this, EventArgs.Empty);
    }
}

public static class SoundInfo
{
    [DllImport("winmm.dll")]
    private static extern uint mciSendString(
        string command,
        StringBuilder returnValue,
        int returnLength,
        IntPtr winHandle);

    public static int GetSoundLength(string fileName)
    {
        StringBuilder lengthBuf = new StringBuilder(32);

        mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
        mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
        mciSendString("close wave", null, 0, IntPtr.Zero);

        int length = 0;
        int.TryParse(lengthBuf.ToString(), out length);

        return length;
    }
}