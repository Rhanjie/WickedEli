using UnityEngine;

namespace Entities.Characters.Players
{
    [CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Settings/Player")]
    public class PlayerSettingsInstaller : LivingEntitySettingsInstaller
    {
        [SerializeField] private Player.Settings playerSettings;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInstance(playerSettings);
        }
    }
}