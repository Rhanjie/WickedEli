using Terrain;
using UnityEngine;
using Zenject;

namespace Characters.Players
{
    public class PlayerReferencesInstaller : MonoInstaller
    {
        [SerializeField] private IsometricObject.References objectSettings;
        [SerializeField] private Character.References characterReferences;
        [SerializeField] private Player.References playerReferences;

        public override void InstallBindings()
        {
            Container.Bind<Renderer>().FromComponentsInChildren();

            Container.BindInstances(objectSettings, characterReferences, playerReferences);
            Container.BindInterfacesAndSelfTo<Player>().AsSingle();
        }
    }
}