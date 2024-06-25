namespace Entities.Characters.Players
{
    public class CharacterReferencesInstaller : LivingEntityReferencesInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInterfacesAndSelfTo<Character>().AsSingle();
        }
    }
}