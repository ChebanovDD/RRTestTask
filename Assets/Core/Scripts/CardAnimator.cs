using Core.Interfaces;
using Core.Models;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public abstract class CardAnimator : MonoBehaviour
    {
        public abstract Sequence SetCardTransform(ICard card, CardTransform cardTransform);
        public abstract Sequence ActivateCard(ICard card);
        public abstract Sequence DeactivateCard(ICard card);
    }
}
