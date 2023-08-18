using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace DeepSlay
{
    public class BattleController : IDisposable
    {
        private SignalBus _signalBus;
        private SpellConfig _spellConfig;
        private DiceViewService _diceViewService;
        private BattlePhaseRepository _battlePhaseRepository;

        public BattleController(
            SignalBus signalBus,
            SpellConfig spellConfig,
            DiceViewService viewService,
            BattlePhaseRepository phaseRepository)
        {
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
            var views = _diceViewService.Views;
            if (views.Count < 2)
            {
                return;
            }

            var spells = _spellConfig.SpellModels;
            for (var i = 0; i < views.Count; i++)
            {
                var currentElement = views[i].Element;

                if (i + 1 == views.Count)
                {
                    break;
                }

                var nextElement = views[i + 1].Element;
                var combination = new List<Elements> { currentElement, nextElement };
                var diceViews = new List<DiceView> { views[i], views[i + 1] };

                var spell = spells.Find(spell => spell.Combinations.Any(comb =>
                    comb.Combination.SequenceEqual(combination)));

                if (spell != null)
                {
                    CombineElements(spell, diceViews);
                    break;
                }
            }
        }

        private void CombineElements(SpellModel spellModel, List<DiceView> views)
        {
            var viewA = views[0];
            var viewB = views[1];

            var spell = _diceViewService.Spawn();
            spell.SetSpell(spellModel.Name);
            spell.transform.position = Vector3.Lerp(viewA.transform.position, viewB.transform.position, 0.5f);

            _diceViewService.DeSpawn(viewA);
            _diceViewService.DeSpawn(viewB);
        }
    }
}