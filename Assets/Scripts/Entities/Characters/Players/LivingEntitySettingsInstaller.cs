using Entities.Characters.Behaviours;
using UnityEngine;

namespace Entities.Characters.Players
{
    [CreateAssetMenu(fileName = "LivingEntityInstaller", menuName = "Settings/LivingEntity")]
    public class LivingEntitySettingsInstaller : StaticEntitySettingsInstaller
    {
        [SerializeField] private MovementBehaviour.Settings movementSettings;
        [SerializeField] private MeleeAttackBehaviour.Settings attackSettings;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInstances(movementSettings, attackSettings);
        }
    }
}