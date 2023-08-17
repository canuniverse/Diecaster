using System;
using System.Collections.Generic;

namespace DeepSlay
{
    [Serializable]
    public class DieModel
    {
        public List<Elements> DieFaces;
    }

    public enum Elements
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
    }
}