using Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Characters.Players
{
    [CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Settings/Player")]
    public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
    {
        [SerializeField] private StaticEntity.Settings characterSettings;
        [SerializeField] private Player.Settings playerSettings;
    
        [SerializeField] private MovementBehaviour.Settings movementSettings;
        [SerializeField] private MeleeAttackBehaviour.Settings attackSettings;
    
        public override void InstallBindings()
        {
            Container.BindInstances(characterSettings, playerSettings);
            Container.BindInstances(movementSettings, attackSettings);
        }
    }
}