using UnityEngine;

namespace DeepSlay
{
    public class CursorService
    {
        private static CursorConfig _cursorConfig;

        public CursorService(CursorConfig cursorConfig)
        {
            _cursorConfig = cursorConfig;
        }

        public static void SetCursor(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.CursorDefault:
                    SetCursor(_cursorConfig.CursorDefault, Vector2.zero);
                    break;
                case CursorType.CursorSelect:
                    SetCursor(_cursorConfig.CursorSelect, Vector2.zero);
                    break;
                case CursorType.CursorSelectTap:
                    SetCursor(_cursorConfig.CursorSelectTap, Vector2.zero);
                    break;
                case CursorType.CursorGrab:
                    var grabCursor = _cursorConfig.CursorGrab;
                    SetCursor(grabCursor, new Vector2(grabCursor.width / 2f, grabCursor.height / 2f));
                    break;
                case CursorType.CursorGrabbing:
                    var grabbingCursor = _cursorConfig.CursorGrabbing;
                    SetCursor(grabbingCursor, new Vector2(grabbingCursor.width / 2f, grabbingCursor.height / 2f));
                    break;
                case CursorType.CursorDiscard:
                    var cursorDiscard = _cursorConfig.CursorDiscard;
                    SetCursor(cursorDiscard, new Vector2(cursorDiscard.width / 2f, cursorDiscard.height / 2f));
                    break;
                case CursorType.CursorAttack:
                    var cursorAttack = _cursorConfig.CursorAttack;
                    SetCursor(cursorAttack, new Vector2(cursorAttack.width / 2f, cursorAttack.height / 2f));
                    break;
            }
        }

        private static void SetCursor(Texture2D texture2D, Vector2 hotSpot, CursorMode cursorMode = CursorMode.Auto)
        {
            Cursor.SetCursor(texture2D, hotSpot, cursorMode);
        }
    }

    public enum CursorType
    {
        CursorDefault,
        CursorSelect,
        CursorSelectTap,
        CursorGrab,
        CursorGrabbing,
        CursorDiscard,
        CursorAttack,
    }
}