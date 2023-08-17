using Zenject;

namespace DeepSlay
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BagRepository>().AsSingle();

            Container.BindInterfacesAndSelfTo<BagController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DiceController>().AsSingle().NonLazy();
        }
    }
}