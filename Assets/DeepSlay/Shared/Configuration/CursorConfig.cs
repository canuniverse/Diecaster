using UnityEngine;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "CursorConfig", menuName = "Configuration/CursorConfig")]
    public class CursorConfig : ScriptableObject
    {
        public Texture2D CursorDefault;
        public Texture2D CursorSelect;
        public Texture2D CursorSelectTap;
        public Texture2D CursorGrab;
        public Texture2D CursorGrabbing;
    }
}