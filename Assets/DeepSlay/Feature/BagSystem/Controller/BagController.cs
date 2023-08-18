using System;
using System.Collections.Generic;

namespace DeepSlay
{
    public class BagController : IDisposable
    {
        private DiceConfig _diceConfig;
        private BagRepository _bagRepository;

        public BagController(
            BagRepository bagRepository,
            DiceConfig diceConfig)
        {
            _diceConfig = diceConfig;
            _bagRepository = bagRepository;

            CreateBag();
        }

        public void Dispose()
        {
        }

        private void CreateBag()
        {
            var copyCount = 2;
            var dieModels = _diceConfig.DieModels;
            
            //manipulated deck
            var fireDie = dieModels.Find(die => die.DieFaces.Contains(Elements.Fire));

            dieModels = new List<DieModel>
            {
                fireDie, fireDie, fireDie, fireDie
            };
            
            var bag = new List<DieModel>();

            foreach (var dieModel in dieModels)
            {
                for (var i = 0; i < copyCount; i++)
                {
                    var clone = (DieModel)ObjectClone.DeepClone(dieModel);
                    bag.Add(clone);
                }
            }

            _bagRepository.DieModels = bag.Randomize();
        }
    }
}