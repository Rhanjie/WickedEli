using Characters.Behaviours;
using Map;
using UI;
using UnityEngine;
using Zenject;

namespace Characters.Players
{
    public class PlayerReferencesInstaller : MonoInstaller
    {
        [SerializeField] private IsometricObject.References objectSettings;
        [SerializeField] private StaticEntity.References entityReferences;

        [SerializeField] private MovementBehaviour.References movementReferences;
        [SerializeField] private MeleeAttackBehaviour.References attackReferences;

        public override void InstallBindings()
        {
            //IsometricObject
            Container.Bind<Renderer>().FromComponentsInChildren().AsTransient();
            Container.Bind<Transform>().FromNewComponentOnRoot().AsCached();
            Container.BindInstance(objectSettings);
            
            //StaticEntity
            Container.BindInstance(entityReferences);
            
            //LivingEntity
            Container.BindInterfacesTo<MovementBehaviour>().AsCached();
            Container.BindInterfacesTo<MeleeAttackBehaviour>().AsCached();
            Container.BindInstances(movementReferences, attackReferences);
            
            //Player
            Container.BindInterfacesAndSelfTo<Player>().AsSingle();
        }
    }
}