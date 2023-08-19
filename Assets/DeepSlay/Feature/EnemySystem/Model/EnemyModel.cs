using System;

namespace DeepSlay
{
    [Serializable]
    public class EnemyModel
    {
        public Elements Element = Elements.Fire;
        public int HP = 10;
        public int MaxHP = 10;
        public int BasicAttackMin = 5;
        public int BasicAttackMax = 10;
        public bool IsStuned = false;
    }
}