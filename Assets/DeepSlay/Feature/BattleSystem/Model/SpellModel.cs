using System;
using System.Collections.Generic;

namespace DeepSlay
{
    [Serializable]
    public class SpellModel
    {
        public string Name;
        public List<CombinationModel> Combinations;
        public bool IsAreaEffect;
        public int DamageValue;
        public int HealValue;
        public bool IsStun;
    }

    [Serializable]
    public class CombinationModel
    {
        public List<Elements> Combination;
    }
}