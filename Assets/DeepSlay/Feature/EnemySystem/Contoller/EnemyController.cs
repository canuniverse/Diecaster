using System;
using UniRx;

namespace DeepSlay
{
    public class EnemyController : IDisposable
    {
        private LevelConfig _levelConfig;
        private EnemyConfig _enemyConfig;
        private EnemyAreaView _enemyAreaView;

        public EnemyController(
            LevelConfig levelConfig,
            EnemyConfig enemyConfig,
            EnemyAreaView enemyAreaView)
        {
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;
            _enemyAreaView = enemyAreaView;

            Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => PickEnemies());
        }

        public void Dispose()
        {
        }

        private void PickEnemies()
        {
            var currentLevel = _levelConfig.LevelModels[0];

            for (var i = 0; i < currentLevel.Enemies.Count; i++)
            {
                var element = currentLevel.Enemies[i];
                var enemy = _enemyConfig.EnemyModels.Find(enemyModel => enemyModel.Element == element);
                _enemyAreaView.EnemyViews[i].SetEnemy(enemy);
            }
        }
    }
}