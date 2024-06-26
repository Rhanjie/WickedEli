using Entities;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Map
{
    public class TerrainReferencesInstaller : MonoInstaller
    {
        public TerrainGeneratorSettings settings;
        public StaticEntity staticEntityPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(settings);
            
            Container.Bind<Tilemap>().FromComponentInChildren().AsSingle();
            Container.Bind<TilemapCollider2D>().FromComponentInChildren().AsSingle();
            Container.Bind<TerrainGenerator>().FromNewComponentOnRoot().AsSingle();
            Container.Bind<StaticEntity>().FromInstance(staticEntityPrefab).AsSingle();
        }
    }
}