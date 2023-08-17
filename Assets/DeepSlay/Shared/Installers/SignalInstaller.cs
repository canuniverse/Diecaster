using Zenject;

namespace DeepSlay
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<DiceBagClickedSignal>();
        }
    }
}