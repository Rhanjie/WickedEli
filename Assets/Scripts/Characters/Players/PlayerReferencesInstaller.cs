using Characters;
using UnityEngine;
using Zenject;
using Terrain;
using Characters.Players;

public class PlayerReferencesInstaller : MonoInstaller
{
    [SerializeField] private IsometricObject.References objectSettings;
    [SerializeField] private Character.References characterReferences;
    [SerializeField] private Player.References playerReferences;

    public override void InstallBindings()
    {
        Container.BindInstances(objectSettings, characterReferences, playerReferences);
        Container.BindInterfacesTo<Character>().AsSingle();
    }
}