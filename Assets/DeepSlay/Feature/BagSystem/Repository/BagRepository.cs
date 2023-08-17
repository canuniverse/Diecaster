using System.Collections.Generic;

namespace DeepSlay
{
    public class BagRepository
    {
        public List<DieModel> DieModels { get; set; } = new ();
        public List<DieModel> DiscardDeck { get; set; } = new ();
    }
}