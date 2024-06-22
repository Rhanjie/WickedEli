using Map;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class StaticEntityReferencesInstaller : MonoInstaller
    {
        [SerializeField] private IsometricObject.References objectSettings;
        [SerializeField] private StaticEntity.References entityReferences;
    
        public override void InstallBindings()
        {
            //IsometricObject
            Container.Bind<Renderer>().FromComponentsInChildren().AsTransient();
            Container.Bind<Transform>().FromNewComponentOnRoot().AsCached();
            Container.BindInstance(objectSettings);
            
            //StaticEntity
            Container.BindInstance(entityReferences);
        }
    }
}
