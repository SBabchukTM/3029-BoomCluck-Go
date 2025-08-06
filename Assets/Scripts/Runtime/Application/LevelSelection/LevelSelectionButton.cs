using Application.Services.Audio;
using Core.Services.Audio;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Animation), typeof(Button))]
public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] private int _levelID = 0;

    [SerializeField] private LevelSelectionButtonComponents _buttonComponents;
    [SerializeField, Header("Optional")] private Image _lockImage;
    [SerializeField] private bool _hideLevelIfLocked = false;
    [SerializeField] private TextMeshProUGUI _errorText;
    [SerializeField] private GameObject _errorGO;
    
    private IAudioService _audioService;

    public event Action<int> OnLevelSelected;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }

    private void OnValidate()
    {
        SetFieldValuesAutomatically();
    }

    private void OnDestroy()
    {
        _buttonComponents.Button.onClick.RemoveAllListeners();
    }

    public void Initialize(bool locked, int cardsRequired)
    {
        UpdateLockedLevelDisplay(locked, cardsRequired);
        InitializeButton(locked);
    }

    public void SetSprite(Sprite sprite) => _buttonComponents.ButtonImage.sprite = sprite;

    public void SetColor(Color color) => _buttonComponents.ButtonImage.color = color;

    private void InitializeButton(bool locked)
    {
        _buttonComponents.Button.interactable = !locked;
        _buttonComponents.Button.onClick.AddListener(() =>
        {
            ProcessClick();
            OnLevelSelected?.Invoke(_levelID);
        });
    }

    private void UpdateLockedLevelDisplay(bool locked, int cardsRequired)
    {
        if(_lockImage != null)
            _lockImage.gameObject.SetActive(locked);
        
        if(_hideLevelIfLocked)
            _buttonComponents.LevelText.gameObject.SetActive(!locked);

        _errorGO.SetActive(locked);
        _errorText.text = "Cards required: " + cardsRequired;
    }

    private void ProcessClick()
    {
        _buttonComponents.PressAnimation.Play();
        _audioService.PlaySound(ConstAudio.PressButtonSound);
    }

    private void SetFieldValuesAutomatically()
    {
        _levelID = transform.GetSiblingIndex();
        _buttonComponents.LevelText.text = "Level" + (_levelID + 1).ToString();
    }

    [Serializable]
    private class LevelSelectionButtonComponents
    {
        public Image ButtonImage;
        public TextMeshProUGUI LevelText;
        public Animation PressAnimation;
        public Button Button;
    }
}
