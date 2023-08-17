using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace DeepSlay
{
    public class DiceController : IDisposable
    {
        private BagRepository _bagRepository;
        
        public DiceController(BagRepository bagRepository)
        {
            _bagRepository = bagRepository;

            Update();
        }

        public void Dispose()
        {
        }

        private void Update()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RollDice();
                }
            });
        }

        private void RollDice()
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
        }
    }
}