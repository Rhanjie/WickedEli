using UnityEngine;

namespace Characters.Players
{
    [CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Settings/Player")]
    public class PlayerSettingsInstaller : LivingEntitySettingsInstaller
    {
        [SerializeField] private Player.Settings playerSettings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings);
        }
    }
}