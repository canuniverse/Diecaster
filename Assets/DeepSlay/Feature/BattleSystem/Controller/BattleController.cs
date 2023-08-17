using System;
using System.Collections.Generic;
using Zenject;

namespace DeepSlay.Feature.BattleSystem.Controller
{
    public class BattleController : IDisposable
    {
        private SignalBus _signalBus;
        private List<Elements> _rolledDice;

        public BattleController(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<DiceSpawnCompletedSignal>(OnDiceSpawnCompletedSignal);
            
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<DiceSpawnCompletedSignal>(OnDiceSpawnCompletedSignal);
        }

        private void OnDiceSpawnCompletedSignal(DiceSpawnCompletedSignal signal)
        {
            _rolledDice = new List<Elements>(signal.RolledFaces);
        }

    }
}