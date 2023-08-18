using System;
using System.Collections.Generic;
using Zenject;

namespace DeepSlay.Feature.BattleSystem.Controller
{
    public class BattleController : IDisposable
    {
        private SignalBus _signalBus;
        private SpellConfig _spellConfig;
        private List<Elements> _rolledDice;
        private BattlePhaseRepository _battlePhaseRepository;

        public BattleController(
            SignalBus signalBus,
            SpellConfig spellConfig,
            BattlePhaseRepository phaseRepository)
        {
            _signalBus = signalBus;
            _spellConfig = spellConfig;
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

            CheckCombinations();
        }

        private void CheckCombinations()
        {
            _battlePhaseRepository.BattlePhase = BattlePhase.Merge;
        }
    }
}