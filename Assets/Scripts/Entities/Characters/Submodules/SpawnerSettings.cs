using UnityEngine;
using Zenject;

namespace Entities.Characters.Submodules
{
    [CreateAssetMenu(fileName = "SpawnerSettings", menuName = "Settings/Spawner")]
    public class SpawnerSettings : ScriptableObjectInstaller<SpawnerSettings>
    {
        [SerializeField] private Spawner.Settings spawnerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(spawnerSettings);
        }
    }
}