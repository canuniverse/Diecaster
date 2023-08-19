using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class EnemyView : PoolView<EnemyView>
    {
        [SerializeField] private Image _iconImage;

        private AtlasConfig _atlasConfig;

        [Inject]
        private void Construct(AtlasConfig atlasConfig)
        {
            _atlasConfig = atlasConfig;
        }

        public void SetEnemy(EnemyModel enemy)
        {
            gameObject.SetActive(true);

            _iconImage.sprite = _atlasConfig.IconAtlas.GetSprite($"{enemy.Element}-1");
            _iconImage.SetNativeSize();
        }
    }
}