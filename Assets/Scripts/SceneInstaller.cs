using UI;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.Bind<HUD>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Map.Terrain>().FromComponentInHierarchy().AsSingle();
    }
}
