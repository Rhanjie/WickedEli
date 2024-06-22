using UnityEngine;

namespace Entities.Characters.Players
{
    [CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Settings/Character")]
    public class CharacterSettingsInstaller : LivingEntitySettingsInstaller
    {
        [SerializeField] private Character.Settings characterSettings;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInstance(characterSettings);
        }
    }
}