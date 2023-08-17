using System;

namespace DeepSlay
{
    public class DiceController : IDisposable
    {
        
        
        
        public DiceController()
        {
        }

        public void Dispose()
        {
        }

        private void RollDice()
        {
            var diceCount = 5;
            
            for (var i = 0; i < diceCount; i++)
            {
                var result = UnityEngine.Random.Range(0, 1);
            }
        }
    }
}