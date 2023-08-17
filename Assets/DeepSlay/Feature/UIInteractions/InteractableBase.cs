using UnityEngine;
using UnityEngine.EventSystems;

namespace DeepSlay
{
    public class InteractableBase : MonoBehaviour, IPointerEnterHandler, 
        IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}