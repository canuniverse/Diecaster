using UnityEngine;
using Zenject;

namespace DeepSlay
{
    [CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
    public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
    {
        public override void InstallBindings()
        {
            var objects = Resources.LoadAll("Configuration", typeof(ScriptableObject));

            foreach (var so in objects)
            {
                Container.Bind(so.GetType())
                    .FromScriptableObject(so as ScriptableObject).AsSingle().NonLazy();
            }
        }
    }
}