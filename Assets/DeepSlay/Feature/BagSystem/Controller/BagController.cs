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
            var waterDie = dieModels.Find(die => die.DieFaces.Contains(Elements.Water));
            var earthDie = dieModels.Find(die => die.DieFaces.Contains(Elements.Earth));
            var airDie = dieModels.Find(die => die.DieFaces.Contains(Elements.Air));



            dieModels = new List<DieModel>
            {
                fireDie, fireDie, waterDie, airDie
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