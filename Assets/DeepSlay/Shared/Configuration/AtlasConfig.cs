using UnityEngine;
using UnityEngine.U2D;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "SpriteAtlasConfig", menuName = "Configuration/SpriteAtlasConfig")]
    public class AtlasConfig : ScriptableObject
    {
        public SpriteAtlas IconAtlas;
    }
}