using LinguaQuest.Web.Services.Interfaces;

namespace LinguaQuest.Web.Services.Timers;

public class CountdownGameTimer : IGameTimer
{
    private int _remainingSeconds;

    public int TotalSeconds { get; private set; }
    public int RemainingSeconds => _remainingSeconds;
    public bool IsRunning { get; private set; }

    public void Start(int totalSeconds)
    {
        TotalSeconds = Math.Max(0, totalSeconds);
        _remainingSeconds = TotalSeconds;
        IsRunning = TotalSeconds > 0;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Tick()
    {
        if (!IsRunning || _remainingSeconds <= 0)
        {
            IsRunning = false;
            return;
        }

        _remainingSeconds--;
        if (_remainingSeconds <= 0)
        {
            IsRunning = false;
        }
    }
}