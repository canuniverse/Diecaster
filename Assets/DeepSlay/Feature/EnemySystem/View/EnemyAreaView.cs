using System.Collections.Generic;
using UnityEngine;

namespace DeepSlay
{
    public class EnemyAreaView : UIView
    {
        [SerializeField] private List<EnemyView> _enemyViews;

        public List<EnemyView> EnemyViews => _enemyViews;
    }
}