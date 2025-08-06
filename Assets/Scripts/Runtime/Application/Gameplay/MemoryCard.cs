using System;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _cardImage;
    [SerializeField] private Sprite _solvedSprite;

    private bool _interactible = true;
    private Sprite _cardSprite;
    
    public string CardName => _cardSprite.name;
    public Sprite CardSprite => _cardSprite;

    public event Action<MemoryCard> OnPressed;
    
    public void Initialize(Sprite sprite)
    {
        _cardSprite = sprite;
        _cardImage.sprite = sprite;
        ShowCardImage(false);
        
        _button.onClick.AddListener(() =>
        {
            if(_interactible)
                OnPressed?.Invoke(this);
        });
    }

    public void SetSolved()
    {
        _button.image.sprite = _solvedSprite;
        _button.interactable = false;
    }
    
    public void SetInteractible(bool value) => _interactible = value;

    public void ShowCardImage(bool show)
    {
        Color targetColor = _cardImage.color;
        targetColor.a = show ? 1f : 0f;
        _cardImage.color = targetColor;
    }
}
