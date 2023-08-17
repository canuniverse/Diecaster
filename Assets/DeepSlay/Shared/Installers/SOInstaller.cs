using UnityEngine;
using Zenject;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
    public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
    {
        [SerializeField] private DiceConfig _diceConfig;

        public override void InstallBindings()
        {
            Container.Bind<DiceConfig>().FromScriptableObject(_diceConfig).AsSingle().NonLazy();
        }
    }
}