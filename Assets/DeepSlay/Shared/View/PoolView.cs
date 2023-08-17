using UnityEngine;
using Zenject;

namespace DeepSlay
{
    public class PoolView<T> : MonoBehaviour
    {
        protected virtual void SetActive()
        {
            gameObject.SetActive(true);
        }

        protected virtual void SetInactive()
        {
            gameObject.SetActive(false);
        }

        public class Pool : MemoryPool<T>
        {
            protected override void OnSpawned(T poolView)
            {
                if (poolView is PoolView<T> view)
                {
                    view.SetActive();
                }
                base.OnSpawned(poolView);
            }

            protected override void OnDespawned(T poolView)
            {
                if (poolView is PoolView<T> view)
                {
                    view.SetInactive();
                }
                base.OnDespawned(poolView);
            }
        }
    }
}