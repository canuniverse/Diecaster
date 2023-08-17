using Zenject;

namespace DeepSlay
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var uiViews = FindObjectsOfType<UIView>(true);
            foreach (var uiView in uiViews)
            {
                Container.Bind(uiView.GetType()).FromComponentsInHierarchy().AsSingle();
            }

            Container.Bind<BagRepository>().AsSingle();

            Container.BindInterfacesAndSelfTo<BagController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DiceController>().AsSingle().NonLazy();
        }
    }
}