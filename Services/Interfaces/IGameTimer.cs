namespace LinguaQuest.Web.Services.Interfaces;

public interface IGameTimer
{
    int TotalSeconds { get; }
    int RemainingSeconds { get; }
    bool IsRunning { get; }
    void Start(int totalSeconds);
    void Stop();
    void Tick();
}