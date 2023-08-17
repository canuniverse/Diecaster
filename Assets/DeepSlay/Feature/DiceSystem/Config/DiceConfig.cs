using System.Collections.Generic;
using UnityEngine;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "DiceConfig", menuName = "Configuration/DiceConfig")]
    public class DiceConfig : ScriptableObject
    {
        public List<DieModel> DieModels;
    }
}