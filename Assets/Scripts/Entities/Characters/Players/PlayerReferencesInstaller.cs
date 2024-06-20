namespace Entities.Characters.Players
{
    public class PlayerReferencesInstaller : LivingEntityReferencesInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInterfacesAndSelfTo<Player>().AsSingle();
        }
    }
}