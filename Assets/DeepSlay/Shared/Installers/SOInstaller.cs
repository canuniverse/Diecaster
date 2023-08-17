using UnityEngine;
using Zenject;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
    public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}