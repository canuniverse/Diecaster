using System;
using UniRx;
using Zenject;

namespace DeepSlay
{
    public class EnemyController : IDisposable
    {
        private SignalBus _signalBus;
        private LevelConfig _levelConfig;
        private EnemyConfig _enemyConfig;
        private SpellConfig _spellConfig;
        private EnemyAreaView _enemyAreaView;

        private SpellModel _spellModel;

        public EnemyController(
            SignalBus signalBus,
            LevelConfig levelConfig,
            EnemyConfig enemyConfig,
            SpellConfig spellConfig,
            EnemyAreaView enemyAreaView)
        {
            _signalBus = signalBus;
            _spellConfig = spellConfig;
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;
            _enemyAreaView = enemyAreaView;

            _signalBus.Subscribe<EnemySelectedSignal>(OnEnemySelected);
            _signalBus.Subscribe<SpellSelectedSignal>(OnSpellSelected);

            Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => PickEnemies());
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemySelectedSignal>(OnEnemySelected);
            _signalBus.Unsubscribe<SpellSelectedSignal>(OnSpellSelected);
        }

        private void OnSpellSelected(SpellSelectedSignal signal)
        {
            _spellModel = _spellConfig.Get(signal.SpellName);
        }

        private void OnEnemySelected(EnemySelectedSignal signal)
        {
            if (_spellModel == null)
            {
                return;
            }

            var enemy = signal.EnemyView;

            enemy.EnemyModel.HP -= _spellModel.DamageValue;
            enemy.ShowHp(_spellModel.DamageValue);

            if (enemy.EnemyModel.HP <= 0)
            {
                enemy.Die();
            }
        }

        private void PickEnemies()
        {
            var currentLevel = _levelConfig.LevelModels[0];

            for (var i = 0; i < currentLevel.Enemies.Count; i++)
            {
                var element = currentLevel.Enemies[i];
                var enemy = _enemyConfig.EnemyModels.Find(enemyModel => enemyModel.Element == element);
                var clone = (EnemyModel)ObjectClone.DeepClone(enemy);
                _enemyAreaView.EnemyViews[i].SetEnemy(clone);
            }
        }
    }
}