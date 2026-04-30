using LinguaQuest.Web.Services.Interfaces;

namespace LinguaQuest.Web.Services.Timers;

public class NoGameTimer : IGameTimer
{
    public int TotalSeconds => 0;
    public int RemainingSeconds => 0;
    public bool IsRunning => false;

    public void Start(int totalSeconds)
    {
    }

    public void Stop()
    {
    }

    public void Tick()
    {
    }
}