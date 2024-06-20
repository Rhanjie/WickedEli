using UnityEngine;
using Zenject;

namespace Characters.Players
{
    [CreateAssetMenu(fileName = "StaticEntityInstaller", menuName = "Settings/StaticEntity")]
    public class StaticEntitySettingsInstaller : ScriptableObjectInstaller<StaticEntitySettingsInstaller>
    {
        [SerializeField] private StaticEntity.Settings staticEntitySettings;

        public override void InstallBindings()
        {
            Container.BindInstance(staticEntitySettings);
        }
    }
}