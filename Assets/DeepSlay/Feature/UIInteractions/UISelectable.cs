using UnityEngine.EventSystems;

namespace DeepSlay
{
    public class UISelectable : InteractableBase
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorSelect);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorDefault);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorSelectTap);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorSelect);
        }
    }
}