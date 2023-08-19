using DG.Tweening;
using UnityEngine;

namespace DeepSlay
{
    public class TiltUIEffect : UIEffectBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            Sequence.Append(transform.DORotate(new Vector3(0, 0, -2), 0.5f).SetEase(Ease.InSine));
            Sequence.Append(transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InSine));
            Sequence.Append(transform.DORotate(new Vector3(0, 0, 2), 0.5f).SetEase(Ease.InSine));
            Sequence.Append(transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InSine));
            Sequence.SetLoops(-1);
            Sequence.Play();
        }
    }
}