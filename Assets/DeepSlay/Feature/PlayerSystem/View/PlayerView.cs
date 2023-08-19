using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class PlayerView : UIView
    {
        private SignalBus _signalBus;

        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _idle;
        [SerializeField] private Sprite _attack;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemySelectedSignal>(OnEnemySelected);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemySelectedSignal>(OnEnemySelected);
        }

        private void OnEnemySelected(EnemySelectedSignal signal)
        {
            _icon.sprite = _attack;
            Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => { _icon.sprite = _idle; });
        }
    }
}