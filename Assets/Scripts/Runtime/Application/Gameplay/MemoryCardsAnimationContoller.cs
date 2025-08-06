using System.Collections.Generic;
using System.Threading;
using Application.Services.Audio;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Application.Gameplay
{
    public class MemoryCardsAnimationContoller
    {
        private readonly IAudioService _audioService;
        
        private const float RotateAnimTime = 0.15f;
        private readonly Vector3 TargetRotation = new Vector3(0f, -90f, 0f);
        private readonly Vector3 DefaultRotation = new Vector3(0f, 0, 0f);

        public MemoryCardsAnimationContoller(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        public async UniTask ShowAllMemoryCards(List<MemoryCard> cards, CancellationToken token)
        {
            _audioService.PlaySound(ConstAudio.CardSound);
            for (int i = 0; i < cards.Count; i++)
                ShowCard(cards[i], token).Forget();
            
            await UniTask.WaitForSeconds(RotateAnimTime, cancellationToken: token);
        }
        
        public async UniTask HideAllMemoryCards(List<MemoryCard> cards, CancellationToken token)
        {
            _audioService.PlaySound(ConstAudio.CardSound);
            for (int i = 0; i < cards.Count; i++)
                HideCard(cards[i], token).Forget();
            
            await UniTask.WaitForSeconds(RotateAnimTime, cancellationToken: token);
        }
        
        public async UniTask ShowCard(MemoryCard card, CancellationToken token)
        {
            card.SetInteractible(false);
            card.transform.DORotate(TargetRotation, RotateAnimTime).SetLink(card.gameObject);
            await UniTask.WaitForSeconds(RotateAnimTime, cancellationToken: token);
        
            card.ShowCardImage(true);
        
            card.transform.DORotate(DefaultRotation, RotateAnimTime).SetLink(card.gameObject);
            await UniTask.WaitForSeconds(RotateAnimTime, cancellationToken: token);
        }

        public async UniTask HideCard(MemoryCard card, CancellationToken token)
        {
            card.transform.DORotate(TargetRotation, RotateAnimTime).SetLink(card.gameObject);
            await UniTask.WaitForSeconds(RotateAnimTime, cancellationToken: token);

            card.ShowCardImage(false);
        
            card.transform.DORotate(DefaultRotation, RotateAnimTime).SetLink(card.gameObject);
            await UniTask.WaitForSeconds(RotateAnimTime, cancellationToken: token);
            card.SetInteractible(true);
        }
    }
}