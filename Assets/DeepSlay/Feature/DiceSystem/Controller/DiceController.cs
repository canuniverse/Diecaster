using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace DeepSlay
{
    public class DiceController : IDisposable
    {
        private SignalBus _signalBus;
        private DiceBagView _bagView;
        private BagRepository _bagRepository;
        private DiceViewService _diceViewService;
        private DiceFaceRepository _diceFaceRepository;
        private BattlePhaseRepository _battlePhaseRepository;

        public DiceController(
            SignalBus signalBus,
            DiceBagView diceBagView,
            DiceViewService diceViewService,
            DiceFaceRepository diceFaceRepository,
            BattlePhaseRepository battlePhaseRepository,
            BagRepository bagRepository)
        {
            _signalBus = signalBus;
            _bagView = diceBagView;
            _bagRepository = bagRepository;
            _diceViewService = diceViewService;
            _diceFaceRepository = diceFaceRepository;
            _battlePhaseRepository = battlePhaseRepository;

            _signalBus.Subscribe<DiscardDiceSignal>(OnDiceDiscarded);
            _signalBus.Subscribe<DiceBagClickedSignal>(OnDiceBagClicked);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<DiscardDiceSignal>(OnDiceDiscarded);
            _signalBus.Unsubscribe<DiceBagClickedSignal>(OnDiceBagClicked);
        }

        private void OnDiceBagClicked(DiceBagClickedSignal signal)
        {
            DrawDices();
        }

        private void OnDiceDiscarded(DiscardDiceSignal signal)
        {
            _diceFaceRepository.ElementsList.RemoveAt(signal.DieIndex);
            _diceViewService.DeSpawn(_diceViewService.Views[signal.DieIndex]);

            _signalBus.Fire(new DiceSpawnCompletedSignal()
            {
                RolledFaces = _diceFaceRepository.ElementsList
            });
        }

        private void DrawDices()
        {
            var diceCount = 5;
            var dices = new List<DieModel>();

            for (var i = 0; i < diceCount; i++)
            {
                if (_bagRepository.DieModels.Count < 1)
                {
                    ReshuffleDiscardDeck();
                }

                var die = _bagRepository.DieModels.First();

                dices.Add(die);
                _bagRepository.DiscardDeck.Add(die);
                _bagRepository.DieModels.Remove(die);
            }

            _battlePhaseRepository.BattlePhase = BattlePhase.Draw;
            
            RollDices(dices);
        }

        private void ReshuffleDiscardDeck()
        {
            var discardDeck = _bagRepository.DiscardDeck.Randomize();

            _bagRepository.DieModels.AddNew(discardDeck);
            _bagRepository.DiscardDeck = new List<DieModel>();
        }

        private void RollDices(List<DieModel> dieModels)
        {
            var resultFaces = new List<Elements>();

            foreach (var dieModel in dieModels)
            {
                var face = dieModel.DieFaces.Random();
                resultFaces.Add(face);
            }

            _diceFaceRepository.ElementsList = resultFaces;

            _battlePhaseRepository.BattlePhase = BattlePhase.Roll;
            
            SpawnDices(resultFaces);
        }

        private void SpawnDices(List<Elements> diceFaces)
        {
            _diceViewService.DespawnAll();

            for (var i = 0; i < diceFaces.Count; i++)
            {
                var face = diceFaces[i];
                var view = _diceViewService.Spawn();
                var parent = _bagView.DiceParents[i];
                view.transform.SetParent(parent, false);
                view.transform.localPosition = Vector3.zero;
                view.SetDieFace(face);
                view.DiceIndex = i;
            }
            
            _battlePhaseRepository.BattlePhase = BattlePhase.Discard;
        }
    }
}