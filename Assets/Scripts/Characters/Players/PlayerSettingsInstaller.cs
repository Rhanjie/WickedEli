using Characters;
using UnityEngine;
using Zenject;
using Characters.Players;

[CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Settings/Player")]
public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
{
    [SerializeField] private LivingEntity.Settings characterSettings;
    [SerializeField] private Player.Settings playerSettings;
    
    public override void InstallBindings()
    {
        Container.BindInstances(characterSettings, playerSettings);
    }
}