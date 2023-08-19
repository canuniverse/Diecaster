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
        private DiceViewService _diceViewService;

        private DiceView _spellView;

        public EnemyController(
            SignalBus signalBus,
            LevelConfig levelConfig,
            EnemyConfig enemyConfig,
            SpellConfig spellConfig,
            DiceViewService diceViewService,
            EnemyAreaView enemyAreaView)
        {
            _signalBus = signalBus;
            _spellConfig = spellConfig;
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;
            _enemyAreaView = enemyAreaView;
            _diceViewService = diceViewService;

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
            _spellView = signal.DiceView;
        }

        private void OnEnemySelected(EnemySelectedSignal signal)
        {
            if (_spellView == null)
            {
                return;
            }

            var spellModel = _spellConfig.Get(_spellView.Spell);

            var enemy = signal.EnemyView;

            enemy.EnemyModel.HP -= spellModel.DamageValue;
            enemy.ShowHp(spellModel.DamageValue);

            if (enemy.EnemyModel.HP <= 0)
            {
                enemy.Die();
            }

            _diceViewService.DeSpawn(_spellView);
            _spellView = null;
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