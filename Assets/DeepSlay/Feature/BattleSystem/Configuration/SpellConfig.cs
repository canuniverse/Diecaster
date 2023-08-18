using System.Collections.Generic;
using UnityEngine;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "SpellConfig", menuName = "Configuration/SpellConfig")]
    public class SpellConfig : ScriptableObject
    {
        public List<SpellModel> SpellModels;
    }
}