using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeepSlay
{
    public class DraggableView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        
        public event Action<PointerEventData> OnDragBegan;
        public event Action<PointerEventData> OnDragged; 
        public event Action<PointerEventData> OnDragEnded;
        
        private void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDragBegan?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var point = Camera.main.ScreenToWorldPoint(eventData.position);
            point.z = 0;
            
            _rectTransform.position = point;
            
            OnDragged?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnded?.Invoke(eventData);
        }
    }
}