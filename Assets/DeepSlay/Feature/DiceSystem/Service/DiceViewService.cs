namespace DeepSlay
{
    public class DiceViewService : PoolService<DiceView>
    {
        protected DiceViewService(DiceView.Pool pool) : base(pool)
        {
        }

        public void DespawnAll()
        {
            var count = Views.Count;
            for (var i = 0; i < count; i++)
            {
                DeSpawn(Views[0]);
            }
        }
    }
}