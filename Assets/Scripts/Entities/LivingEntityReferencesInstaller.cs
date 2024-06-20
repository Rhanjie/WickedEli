using Characters.Behaviours;
using UnityEngine;

namespace Entities
{
    public class LivingEntityReferencesInstaller : StaticEntityReferencesInstaller
    {
        [SerializeField] private MovementBehaviour.References movementReferences;
        [SerializeField] private MeleeAttackBehaviour.References attackReferences;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInterfacesTo<MovementBehaviour>().AsCached();
            Container.BindInterfacesTo<MeleeAttackBehaviour>().AsCached();
            Container.BindInstances(movementReferences, attackReferences);
        }
    }
}