using System.Collections.Generic;
using UnityEngine;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "SpellConfig", menuName = "Configuration/SpellConfig")]
    public class SpellConfig : ScriptableObject
    {
        public List<SpellModel> SpellModels;

        public SpellModel Get(string spellName)
        {
            return SpellModels.Find(model => model.Name == spellName);
        }
    }
}