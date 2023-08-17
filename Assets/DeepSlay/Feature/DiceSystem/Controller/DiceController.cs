using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace DeepSlay
{
    public class DiceController : IDisposable
    {
        private SignalBus _signalBus;
        private DiceBagView _bagView;
        private BagRepository _bagRepository;
        private DiceViewService _diceViewService;

        public DiceController(
            SignalBus signalBus,
            DiceBagView diceBagView,
            DiceViewService diceViewService,
            BagRepository bagRepository)
        {
            _signalBus = signalBus;
            _bagView = diceBagView;
            _bagRepository = bagRepository;
            _diceViewService = diceViewService;

            _signalBus.Subscribe<DiceBagClickedSignal>(OnDiceBagClicked);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<DiceBagClickedSignal>(OnDiceBagClicked);
        }

        private void OnDiceBagClicked(DiceBagClickedSignal signal)
        {
            DrawDices();
        }

        private void DrawDices()
        {
            var bag = _bagRepository.DieModels;

            var diceCount = 5;
            var dices = new List<DieModel>();

            for (var i = 0; i < diceCount; i++)
            {
                if (bag.Count < 1)
                {
                    return;
                }

                var die = bag.First();

                dices.Add(die);
                _bagRepository.DiscardDeck.Add(die);
                _bagRepository.DieModels.Remove(die);
            }

            RollDices(dices);
        }

        private void RollDices(List<DieModel> dieModels)
        {
            var resultFaces = new List<Elements>();

            foreach (var dieModel in dieModels)
            {
                var face = dieModel.DieFaces.Random();
                resultFaces.Add(face);
            }

            SpawnDices(resultFaces);
        }

        private void SpawnDices(List<Elements> diceFaces)
        {
            for (var i = 0; i < diceFaces.Count; i++)
            {
                var face = diceFaces[i];
                var view = _diceViewService.Spawn();
                var parent = _bagView.DiceParents[i];
                view.transform.SetParent(parent, false);
                view.SetDieFace(face);
            }
        }
    }
}