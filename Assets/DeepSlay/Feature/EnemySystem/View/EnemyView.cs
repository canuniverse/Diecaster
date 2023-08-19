using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class EnemyView : PoolView<EnemyView>, IPointerEnterHandler,
        IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _hpBar;

        private SignalBus _signalBus;
        private AtlasConfig _atlasConfig;
        private BattlePhaseRepository _phaseRepository;
        
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

        public void ShowHp()
        {
            _hpBar.fillAmount = EnemyModel.HP / (float)EnemyModel.MaxHP;
        }
        
        public void Die()
        {
            gameObject.SetActive(false);
        }

        public void SetEnemy(EnemyModel enemy)
        {
            EnemyModel = enemy;
            
            gameObject.SetActive(true);

            _iconImage.sprite = _atlasConfig.IconAtlas.GetSprite($"{enemy.Element}-1");
            _iconImage.SetNativeSize();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.SelectTarget)
            {
                CursorService.SetCursor(CursorType.CursorAttack);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorDefault);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.SelectTarget)
            {
                _signalBus.Fire(new EnemySelectedSignal { EnemyView = this });
            }
        }
    }
}