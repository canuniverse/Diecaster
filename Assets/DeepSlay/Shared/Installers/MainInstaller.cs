using UnityEngine;
using Zenject;

namespace DeepSlay
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private DiceView _diceView;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<DiceView, DiceView.Pool>().FromComponentInNewPrefab(_diceView);

            var uiViews = FindObjectsOfType<UIView>(true);
            foreach (var uiView in uiViews)
            {
                Container.Bind(uiView.GetType())
                    .FromComponentsInHierarchy().AsSingle();
            }

            Container.Bind<DiceViewService>().AsSingle();

            Container.Bind<BagRepository>().AsSingle();

            Container.BindInterfacesAndSelfTo<BagController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DiceController>().AsSingle().NonLazy();
        }
    }
}