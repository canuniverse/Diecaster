using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class DiceBagView : UIView
    {
        [SerializeField] private Button _bagButton;
        [SerializeField] private List<Transform> _diceParents;
        [SerializeField] private List<Transform> _spellParents;

        public List<Transform> DiceParents => _diceParents;
        public List<Transform> SpellParents => _spellParents;

        private SignalBus _signalBus;
        private BattlePhaseRepository _battlePhaseRepository;

        [Inject]
        private void Construct(
            SignalBus signalBus,
            BattlePhaseRepository phaseRepository)
        {
            _signalBus = signalBus;
            _battlePhaseRepository = phaseRepository;
        }

        private void OnEnable()
        {
            _bagButton.onClick.AddListener(OnBagButtonClicked);
        }

        private void OnDisable()
        {
            _bagButton.onClick.RemoveListener(OnBagButtonClicked);
        }

        private void OnBagButtonClicked()
        {
            if (_battlePhaseRepository.BattlePhase == BattlePhase.Draw)
            {
                _signalBus.Fire(new DiceBagClickedSignal());
            }
        }
    }
}