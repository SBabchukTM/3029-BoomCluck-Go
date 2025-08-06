using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Infrastructure.Timer.SpecificTimers;

public class GameplayTimer
{
    private readonly GameData _gameData;

    public GameplayTimer(GameData gameData)
    {
        _gameData = gameData;
    }

    public async UniTask RunTimer(float levelTime, CancellationToken token)
    {
        CountdownTimer timer = new CountdownTimer(levelTime);

        timer.Start();
        
        while (!token.IsCancellationRequested && timer.CurrentTime >= 0)
        {
            await UniTask.NextFrame(cancellationToken: token);
            _gameData.TimeLeft = timer.CurrentTime;
        }
        
        _gameData.TimeLeft = 0;
    }
}
