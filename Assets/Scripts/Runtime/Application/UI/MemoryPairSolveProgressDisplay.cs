using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryPairSolveProgressDisplay : MonoBehaviour
{
    [SerializeField] private Image _cardImage;
    [SerializeField] private TextMeshProUGUI _progressText;

    private int _currentProgress = 0;
    private int _targetPairs;
    
    public Sprite CardSprite => _cardImage.sprite;
    
    public void Initialize(Sprite cardSprite, int targetPairs)
    {
        _cardImage.sprite = cardSprite;
        
        _targetPairs = targetPairs;
        _progressText.text = $"0/{targetPairs}";
    }

    public void UpdateProgress()
    {
        _currentProgress++;
        _progressText.text = $"{_currentProgress}/{_targetPairs}";
    }
}
