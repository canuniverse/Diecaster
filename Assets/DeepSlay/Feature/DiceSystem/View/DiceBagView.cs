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

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
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
            _signalBus.Fire(new DiceBagClickedSignal());
        }
    }
}