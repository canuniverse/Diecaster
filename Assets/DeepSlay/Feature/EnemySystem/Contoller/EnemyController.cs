using System;
using System.Linq;
using UniRx;
using Zenject;
using Random = UnityEngine.Random;

namespace DeepSlay
{
    public class EnemyController : IDisposable
    {
        private SignalBus _signalBus;
        private PlayerView _playerView;
        private LevelConfig _levelConfig;
        private EnemyConfig _enemyConfig;
        private SpellConfig _spellConfig;
        private EnemyAreaView _enemyAreaView;
        private DiceViewService _diceViewService;
        private BattleResultView _battleResultView;
        private BattlePhaseRepository _battlePhaseRepository;

        private DiceView _spellView;

        public EnemyController(
            SignalBus signalBus,
            LevelConfig levelConfig,
            EnemyConfig enemyConfig,
            SpellConfig spellConfig,
            PlayerView playerView,
            DiceViewService diceViewService,
            BattleResultView battleResultView,
            BattlePhaseRepository battlePhaseRepository,
            EnemyAreaView enemyAreaView)
        {
            _signalBus = signalBus;
            _playerView = playerView;
            _spellConfig = spellConfig;
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;
            _enemyAreaView = enemyAreaView;
            _diceViewService = diceViewService;
            _battleResultView = battleResultView;
            _battlePhaseRepository = battlePhaseRepository;

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

            if (spellModel.IsStun)
            {
                enemy.EnemyModel.IsStuned = true;
            }
            
            if (!spellModel.IsAreaEffect)
            {
                enemy.EnemyModel.HP -= spellModel.DamageValue;
                enemy.ShowHp(spellModel.DamageValue);
            }
            else
            {
                foreach (var enemyView in _enemyAreaView.EnemyViews)
                {
                    if (enemyView.gameObject.activeSelf)
                    {
                        enemyView.EnemyModel.HP -= spellModel.DamageValue;
                        enemyView.ShowHp(spellModel.DamageValue);
                    }
                }

            }
            
            if (spellModel.HealValue > 0)
            {
                _playerView.HP += spellModel.HealValue;
                _playerView.ShowHp(-spellModel.HealValue);
            }

            if (enemy.EnemyModel.HP <= 0)
            {
                enemy.Die();
            }

            _diceViewService.DeSpawn(_spellView);
            _spellView = null;
            
            var enemies = _enemyAreaView.EnemyViews.FindAll(view => view.gameObject.activeSelf);
            if (enemies.Count < 1)
            {
                _battleResultView.ShowResult(true);
                return;
            }

            var diceViews = _diceViewService.Views;
            var spells = diceViews.FindAll(view => !string.IsNullOrEmpty(view.Spell));
            if (spells.Count < 1)
            {
                EnemyAttacks();
            }
        }

        public void EnemyAttacks()
        {
            var attackDelay = 0.5f;

            var views = _enemyAreaView.EnemyViews
                .Where(view => view.gameObject.activeSelf && view.EnemyModel != null).ToList();

            foreach (var enemy in views)
            {
                var model = enemy.EnemyModel;
                if (model.IsStuned)
                {
                    continue;
                }
                var damage = Random.Range(model.BasicAttackMin, model.BasicAttackMax);

                Observable.Timer(TimeSpan.FromSeconds(attackDelay)).Subscribe(_ =>
                {
                    _playerView.HP -= damage;
                    _playerView.ShowHp(damage);
                });

                attackDelay += 0.5f;
            }

            Observable.Timer(TimeSpan.FromSeconds(attackDelay)).Subscribe(_ =>
            {
                if (_playerView.HP <= 0)
                {
                    _battleResultView.ShowResult(false);
                }
                
                _battlePhaseRepository.BattlePhase = BattlePhase.Draw;
            });
        }

        private void PickEnemies()
        {
            var currentLevel = _levelConfig.LevelModels[_levelConfig.Level];

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