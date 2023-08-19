using System.Collections.Generic;
using UnityEngine;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configuration/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public List<EnemyModel> EnemyModels;
    }
}