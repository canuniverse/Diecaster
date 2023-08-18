using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace DeepSlay
{
    public class BattleController : IDisposable
    {
        private SignalBus _signalBus;
        private DiceBagView _bagView;
        private SpellConfig _spellConfig;
        private DiceViewService _diceViewService;
        private BattlePhaseRepository _battlePhaseRepository;

        public BattleController(
            SignalBus signalBus,
            DiceBagView bagView,
            SpellConfig spellConfig,
            DiceViewService viewService,
            BattlePhaseRepository phaseRepository)
        {
            _bagView = bagView;
            _signalBus = signalBus;
            _spellConfig = spellConfig;
            _diceViewService = viewService;
            _battlePhaseRepository = phaseRepository;
            _signalBus.Subscribe<DiceSpawnCompletedSignal>(OnDiceSpawnCompletedSignal);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<DiceSpawnCompletedSignal>(OnDiceSpawnCompletedSignal);
        }

        private void OnDiceSpawnCompletedSignal(DiceSpawnCompletedSignal signal)
        {
            RefineRolledList();
            CheckCombinations();
        }

        private void RefineRolledList()
        {
            _battlePhaseRepository.BattlePhase = BattlePhase.Merge;

            var noneViews = _diceViewService.Views.FindAll(view => view.Element == Elements.None);

            foreach (var view in noneViews)
            {
                _diceViewService.DeSpawn(view);
            }
        }

        private void CheckCombinations()
        {
            var spells = _spellConfig.SpellModels;
            
            var views = _diceViewService.Views;
            if (views.Count < 2)
            {
                return;
            }

            var group1 = new List<DiceView> { views[0], views[1] };
            var group2 = views.Count > 2 ? new List<DiceView> { views[1], views[2] } : null;
            var group3 = views.Count > 3 ? new List<DiceView> { views[2], views[3] } : null;
            
            var spell1 = spells.Find(spell => spell.Combinations.Any(comb =>
                comb.Combination.SequenceEqual(group1.Select(e => e.Element).ToList())));
            
            var spell2 = group2 == null ? null : spells.Find(spell => spell.Combinations.Any(comb =>
                comb.Combination.SequenceEqual(group2.Select(e => e.Element).ToList())));
            
            var spell3 = group3 == null ? null : spells.Find(spell => spell.Combinations.Any(comb =>
                comb.Combination.SequenceEqual(group3.Select(e => e.Element).ToList())));

            if (spell1 != null)
            {
                CombineElements(spell1, group1, _bagView.SpellParents[0]);
            }
            
            if (spell1 == null && spell2 != null)
            {
                CombineElements(spell2, group2, _bagView.SpellParents[0]);
            }
            
            if (spell3 != null)
            {
                CombineElements(spell3, group3, _bagView.SpellParents[1]);
            }
        }

        private void CombineElements(SpellModel spellModel, List<DiceView> views, Transform parent)
        {
            var viewA = views[0];
            var viewB = views[1];

            var sequence = DOTween.Sequence();

            var position = parent.position;
            
            sequence.Append(viewA.transform.DOMove(position, 0.5f).SetEase(Ease.InSine));
            sequence.Append(viewB.transform.DOMove(position, 0.5f).SetEase(Ease.InSine));

            sequence.OnComplete(() =>
            {
                _diceViewService.DeSpawn(viewA);
                _diceViewService.DeSpawn(viewB);
                
                var spell = _diceViewService.Spawn();
                spell.SetSpell(spellModel.Name);
            
                spell.transform.position = position;
                spell.transform.SetParent(parent);
            });
        }
    }
}