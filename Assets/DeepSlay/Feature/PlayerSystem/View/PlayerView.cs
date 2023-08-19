using System;
using DG.Tweening;
using TMPro;
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

        [SerializeField] private Image _hpBar;
        [SerializeField] private TMP_Text _hpText;

        private Tweener _sliderTween;

        public int HP = 40;
        public int MaxHP = 40;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemySelectedSignal>(OnEnemySelected);

            _hpBar.fillAmount = 1;
            _hpText.SetText($"{HP}/{MaxHP}");
        }

        private void OnDisable()
        {
            _sliderTween?.Kill();
            
            _signalBus.Unsubscribe<EnemySelectedSignal>(OnEnemySelected);
        }

        public void ShowHp(int damage)
        {
            var from = (HP + damage) / (float)MaxHP;
            var to = HP / (float)MaxHP;

            _sliderTween = DOVirtual
                .Float(from, to, 0.2f, value => { _hpBar.fillAmount = value; })
                .OnKill(() => _hpBar.fillAmount = to);

            _hpText.SetText($"{HP}/{MaxHP}");
        }

        private void OnEnemySelected(EnemySelectedSignal signal)
        {
            _icon.sprite = _attack;
            Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => { _icon.sprite = _idle; });
        }
    }
}