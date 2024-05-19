using Characters.Behaviours;
using Map;
using UnityEngine;
using Zenject;

namespace Characters.Players
{
    public class PlayerReferencesInstaller : MonoInstaller
    {
        [SerializeField] private IsometricObject.References objectSettings;
        [SerializeField] private StaticEntity.References entityReferences;
        [SerializeField] private Player.References playerReferences;

        [SerializeField] private MovementBehaviour.References movementReferences;
        [SerializeField] private MeleeAttackBehaviour.References attackReferences;

        public override void InstallBindings()
        {
            //IsometricObject
            Container.Bind<Renderer>().FromComponentsInChildren().AsTransient();
            Container.Bind<Transform>().FromNewComponentOnRoot().AsCached();

            //LivingEntity
            Container.BindInterfacesTo<MovementBehaviour>().AsCached();
            Container.BindInterfacesTo<MeleeAttackBehaviour>().AsCached();

            //IsometricObject, StaticEntity, Player
            Container.BindInstances(objectSettings, entityReferences, playerReferences);
            Container.BindInstances(movementReferences, attackReferences);

            Container.BindInterfacesAndSelfTo<Player>().AsSingle();
        }
    }
}