using Zenject;

namespace DeepSlay
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CursorService>().AsSingle().NonLazy();
        }
    }
}