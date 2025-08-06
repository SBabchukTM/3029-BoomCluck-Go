using System;

public class GameData
{
    private int _levelID = 0;

    private bool _gameEnded = false;
    private int _pairsSolved;
    private int _pairsTarget;
    
    private int _reward = 0;
    
    private float _timeLeft;

    public event Action<int> OnPairSolved;
    public event Action<float> OnTimeChanged;
    
    public int LevelID
    {
        get => _levelID;
        set => _levelID = value;
    }

    public int PairsSolved
    {
        get => _pairsSolved;
        set
        {
            _pairsSolved = value;
            OnPairSolved?.Invoke(value);
        }
    }

    public int PairsTarget
    {
        get => _pairsTarget;
        set => _pairsTarget = value;
    }

    public float TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = value;
            OnTimeChanged?.Invoke(value);
        }
    }

    public bool GameEnded
    {
        get => _gameEnded;
        set => _gameEnded = value;
    }

    public int Reward
    {
        get => _reward;
        set => _reward = value;
    }
}
