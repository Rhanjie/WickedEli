using Characters;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Settings/Player")]
public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
{
    [SerializeField] private Character.Settings characterSettings;
    [SerializeField] private Player.Settings playerSettings;
    
    public override void InstallBindings()
    {;
        Container.BindInstances(characterSettings, playerSettings);
    }
}