using Core.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class CardAnimator : MonoBehaviour
    {
        [SerializeField] private float _activateDuration = 0.4f;
        [SerializeField] private float _deactivateDuration = 0.4f;
        [SerializeField] private Ease _activateEaseType;
        [SerializeField] private Ease _deactivateEaseType;
        [SerializeField] private RectTransform _activeCardTargetPosition;


        public Sequence ActivateCard(ICard card)
        {
            return DOTween.Sequence()
                .Join(card.GameObject.transform.DOMoveY(_activeCardTargetPosition.transform.position.y,
                    _activateDuration))
                .Join(card.GameObject.transform.DORotateQuaternion(Quaternion.identity, _activateDuration))
                .SetEase(_activateEaseType);
        }

        public Sequence DeactivateCard(ICard card)
        {
            return DOTween.Sequence()
                .Join(card.GameObject.transform.DOMoveY(card.StackPosition.y, _deactivateDuration))
                .Join(card.GameObject.transform.DORotateQuaternion(card.StackRotation, _deactivateDuration))
                .SetEase(_deactivateEaseType);
        }
    }
}