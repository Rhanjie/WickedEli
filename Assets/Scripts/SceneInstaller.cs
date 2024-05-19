using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Map.Terrain>().FromComponentInHierarchy().AsSingle();
    }
}
