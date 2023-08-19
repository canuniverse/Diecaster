using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class EnemyView : PoolView<EnemyView>
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _hpBar;
        [SerializeField] private TMP_Text _hpText;

        private SignalBus _signalBus;
        private AtlasConfig _atlasConfig;
        private BattlePhaseRepository _phaseRepository;

        private Sequence Sequence;

        private Tweener _sliderTween;

        public EnemyModel EnemyModel { get; private set; }

        [Inject]
        private void Construct(
            SignalBus signalBus,
            AtlasConfig atlasConfig,
            BattlePhaseRepository phaseRepository)
        {
            _signalBus = signalBus;
            _atlasConfig = atlasConfig;
            _phaseRepository = phaseRepository;
        }

        private void OnDisable()
        {
            Sequence?.Kill();
            _sliderTween?.Kill();
        }

        public void ShowHp(int damage)
        {
            var from = (EnemyModel.HP + damage) / (float)EnemyModel.MaxHP;
            var to = EnemyModel.HP / (float)EnemyModel.MaxHP;

            _sliderTween = DOVirtual
                .Float(from, to, 0.2f, value => { _hpBar.fillAmount = value; })
                .OnKill(() => _hpBar.fillAmount = to);

            _hpText.SetText($"{EnemyModel.HP}/{EnemyModel.MaxHP}");
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }

        public void SetEnemy(EnemyModel enemy)
        {
            EnemyModel = enemy;

            gameObject.SetActive(true);

            ShowIcon($"{enemy.Element}-1");
            _iconImage.SetNativeSize();

            if (enemy.Element != Elements.Air)
            {
                PlayAnimations();
            }
            else
            {
                PlayAirAnimations();
            }

            _hpBar.fillAmount = 1;
            _hpText.SetText($"{EnemyModel.HP}/{EnemyModel.MaxHP}");
        }

        private void ShowIcon(string iconName)
        {
            var sprite = _atlasConfig.IconAtlas.GetSprite(iconName);
            _iconImage.sprite = sprite;
        }

        private void PlayAirAnimations()
        {
            Sequence = DOTween.Sequence();

            var position = transform.position;

            var buffer = 0.3f;
            var movement = 0.05f;

            Sequence.Append(_iconImage.transform.DOMoveY(position.y + buffer, 0).SetEase(Ease.Linear));
            Sequence.Append(_iconImage.transform.DOMoveY(position.y + buffer + movement, 0.5f).SetEase(Ease.Linear));
            Sequence.Append(_iconImage.transform.DOMoveY(position.y + buffer - movement, 0.5f).SetEase(Ease.Linear));
            Sequence.Append(_iconImage.transform.DOMoveY(position.y + buffer, 0.5f).SetEase(Ease.Linear));

            Sequence.SetLoops(-1);
            Sequence.Play();
        }

        private void PlayAnimations()
        {
            Sequence = DOTween.Sequence();

            Sequence.Append(DOVirtual.DelayedCall(0.5f, () => ShowIcon($"{EnemyModel.Element}-1")));
            Sequence.Append(DOVirtual.DelayedCall(0.5f, () => ShowIcon($"{EnemyModel.Element}-2")));
            Sequence.SetLoops(-1);
            Sequence.Play();
        }
    }
}