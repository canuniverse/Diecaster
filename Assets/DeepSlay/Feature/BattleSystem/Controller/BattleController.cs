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
        private List<Elements> _rolledDice;
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
            _rolledDice = new List<Elements>(signal.RolledFaces);

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

            _rolledDice.RemoveAll(view => view == Elements.None);
        }

        private void CheckCombinations()
        {
            if (_rolledDice.Count < 2)
            {
                return;
            }

            var spells = _spellConfig.SpellModels;
            for (var i = 0; i < _rolledDice.Count; i++)
            {
                var currentElement = _rolledDice[i];

                if (i + 1 == _rolledDice.Count)
                {
                    break;
                }

                var nextElement = _rolledDice[i + 1];
                var combination = new List<Elements> { currentElement, nextElement };
                
                var spell = spells.Find(spell => spell.Combinations.Any(comb => 
                    comb.Combination.SequenceEqual(combination)));

                if (spell != null)
                {
                    Debug.Log(spell.Name);
                    break;
                }
            }
        }
    }
}