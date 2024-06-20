using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Map
{
    public class TerrainReferencesInstaller : MonoInstaller
    {
        public TerrainGeneratorSettings settings;
        public Transform staticEntityPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(settings);

            Container.Bind<Tilemap>().FromComponentInChildren().AsSingle();
            Container.Bind<TilemapCollider2D>().FromComponentInChildren().AsSingle();
            Container.Bind<TerrainGenerator>().FromNewComponentOnRoot().AsSingle();
            Container.Bind<Transform>().FromInstance(staticEntityPrefab).AsSingle();
        }
    }
}