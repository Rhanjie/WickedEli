using UnityEngine;
using Zenject;

namespace Terrain
{
    [CreateAssetMenu(fileName = "TerrainGeneratorSettingsInstaller", menuName = "Settings/Terrain")]
    public class TerrainGeneratorSettingsInstaller : ScriptableObjectInstaller<TerrainGeneratorSettingsInstaller>
    {
        public TerrainGenerator.Settings settings;
    
        public override void InstallBindings()
        {;
            Container.BindInstance(settings);
        }
    }
}