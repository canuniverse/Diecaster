using System.Collections.Generic;
using Zenject;

namespace DeepSlay
{
    public class PoolService<T>
    {
        private MemoryPool<T> _pool;
        public List<T> Views { get; set; } = new ();

        protected PoolService(MemoryPool<T> pool)
        {
            _pool = pool;
        }

        public T Spawn()
        {
            var view = _pool.Spawn();
            Views.Add(view);
            return view;
        }

        public void DeSpawn(T view)
        {
            _pool.Despawn(view);
            Views.Remove(view);
        }
    }
}