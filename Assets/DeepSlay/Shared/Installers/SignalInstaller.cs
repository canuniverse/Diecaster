using Zenject;

namespace DeepSlay
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<DiscardDiceSignal>();
            Container.DeclareSignal<SpellSelectedSignal>();
            Container.DeclareSignal<EnemySelectedSignal>();
            Container.DeclareSignal<DiceBagClickedSignal>();
            Container.DeclareSignal<DiceSpawnCompletedSignal>();
        }
    }
}