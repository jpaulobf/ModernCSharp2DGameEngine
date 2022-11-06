using System.Runtime.InteropServices;

namespace HighResolutionTimer;

/**
 * Author:      Mike Zboray - https://github.com/mzboray/HighPrecisionTimer
 * Description: Class Wrapper for the WinMM.dll
 */
public class WinMMWrapper
{
    [DllImport("WinMM.dll", SetLastError = true)]
    public static extern uint timeSetEvent(int msDelay, int msResolution,
        TimerEventHandler handler, ref int userCtx, int eventType);

    public delegate void TimerEventHandler(uint id, uint msg, ref int userCtx,
        int rsv1, int rsv2);

    public enum TimerEventType
    {
        OneTime = 0,
        Repeating = 1
    }

    private readonly Action _elapsedAction;
    private readonly int _elapsedMs;
    private readonly int _resolutionMs;
    private readonly TimerEventType _timerEventType;
    private readonly TimerEventHandler _timerEventHandler;

    public WinMMWrapper(int elapsedMs, int resolutionMs, TimerEventType timerEventType, Action elapsedAction)
    {
        _elapsedMs = elapsedMs;
        _resolutionMs = resolutionMs;
        _timerEventType = timerEventType;
        _elapsedAction = elapsedAction;
        _timerEventHandler = TickHandler;
    }

    public uint StartElapsedTimer()
    {
        var myData = 1; //dummy data
        return timeSetEvent(_elapsedMs, _resolutionMs / 10, _timerEventHandler, ref myData, (int)_timerEventType);
    }

    private void TickHandler(uint id, uint msg, ref int userctx, int rsv1, int rsv2)
    {
        _elapsedAction();
    }
}