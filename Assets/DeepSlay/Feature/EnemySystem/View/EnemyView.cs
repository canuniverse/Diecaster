using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class EnemyView : PoolView<EnemyView>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _iconImage;

        private AtlasConfig _atlasConfig;
        private BattlePhaseRepository _phaseRepository;

        [Inject]
        private void Construct(
            AtlasConfig atlasConfig,
            BattlePhaseRepository phaseRepository)
        {
            _atlasConfig = atlasConfig;
            _phaseRepository = phaseRepository;
        }

        public void SetEnemy(EnemyModel enemy)
        {
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
    }
}