using System;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class DiceView : PoolView<DiceView>, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _diceName;

        public int DiceIndex { get; set; }
        public Elements Element { get; set; }

        private SignalBus _signalBus;
        private BattlePhaseRepository _phaseRepository;
        
        [Inject]
        private void Construct(
            SignalBus signalBus,
            BattlePhaseRepository phaseRepository)
        {
            _signalBus = signalBus;
            _phaseRepository = phaseRepository;
        }

        public void SetDieFace(Elements element)
        {
            var dieNames = Enum.GetNames(typeof(Elements)).ToList();

            dieNames = dieNames.Randomize();

            var time = 0f;

            foreach (var dieName in dieNames)
            {
                Observable.Timer(TimeSpan.FromSeconds(time)).Subscribe(_ =>
                {
                    _diceName.SetText($"{dieName}");
                });

                time += 0.1f;
            }

            Observable.Timer(TimeSpan.FromSeconds(time + 0.1f)).Subscribe(_ =>
            {
                _diceName.SetText($"{element}");
            });

            Element = element;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.Discard)
            {
                CursorService.SetCursor(CursorType.CursorDiscard);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorDefault);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase != BattlePhase.Discard)
            {
                return;
            }
            
            CursorService.SetCursor(CursorType.CursorDefault);
            _signalBus.Fire(new DiscardDiceSignal{ DieIndex = DiceIndex });
        }
    }
}