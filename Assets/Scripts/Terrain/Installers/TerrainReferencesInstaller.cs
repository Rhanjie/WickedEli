using UnityEngine;
using Zenject;

namespace Terrain.Installers
{
    public class TerrainReferencesInstaller : MonoInstaller
    {
        [SerializeField] private TerrainGenerator.References references;

        public override void InstallBindings()
        {
            Container.BindInstance(references);
        }
    }
}