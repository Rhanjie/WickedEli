using Characters.Behaviours;
using Characters.Interfaces;
using Terrain;
using UnityEngine;
using Zenject;

namespace Characters.Players
{
    public class PlayerReferencesInstaller : MonoInstaller
    {
        [SerializeField] private IsometricObject.References objectSettings;
        [SerializeField] private StaticEntity.References entityReferences;
        [SerializeField] private Player.References playerReferences;

        public override void InstallBindings()
        {
            //IsometricObject
            Container.Bind<Renderer>().FromComponentsInChildren().AsTransient();
            Container.Bind<Transform>().FromComponentSibling().AsCached();

            //LivingEntity
            Container.Bind<IMovementBehaviour>().To<MovementBehaviour>().AsTransient();
            Container.Bind<IAttackBehaviour>().To<MeleeAttackBehaviour>().AsTransient();

            //IsometricObject, StaticEntity, Player
            Container.BindInstances(objectSettings, entityReferences, playerReferences);
            Container.BindInterfacesAndSelfTo<Player>().AsSingle();
        }
    }
}