using DG.Tweening;
using UnityEngine;

namespace DeepSlay
{
    public class UIEffectBase : MonoBehaviour
    {
        protected Sequence Sequence;

        protected virtual void OnEnable()
        {
            Sequence = DOTween.Sequence();
        }

        protected virtual void OnDisable()
        {
            Sequence.Kill();
        }
    }
}