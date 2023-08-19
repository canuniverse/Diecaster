using System.Collections.Generic;
using UnityEngine;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configuration/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelModel> LevelModels;
    }
}