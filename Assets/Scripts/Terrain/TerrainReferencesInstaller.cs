using Sirenix.OdinInspector;
using Terrain.Noises;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Terrain.Installers
{
    public class TerrainReferencesInstaller : MonoInstaller
    {
        public TerrainGeneratorSettings settings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(settings);
            
            Container.Bind<Tilemap>().FromComponentInChildren().AsSingle();
            Container.Bind<TerrainGenerator>().FromNewComponentOnRoot().AsSingle();
        }
    }
}