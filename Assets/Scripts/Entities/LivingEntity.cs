using Entities.Characters.Interfaces;
using Map;
using UnityEngine;
using Zenject;
using Terrain = Map.Terrain;

namespace Entities
{
    public class LivingEntity : StaticEntity
    {
        protected IAttackBehaviour AttackBehaviour;
        protected IMovementBehaviour MovementBehaviour;
        
        protected Terrain TerrainHandler;

        private static readonly int VelocityHash = Animator.StringToHash("Velocity");

        [Inject]
        public void Construct(IMovementBehaviour movementBehaviour, IAttackBehaviour attackBehaviour, Terrain terrain)
        {
            MovementBehaviour = movementBehaviour;
            AttackBehaviour = attackBehaviour;
            TerrainHandler = terrain;
        }
        
        protected override void Update()
        {
            base.Update();

            if (MovementBehaviour.Velocity > 0)
                UpdateTileBelow();

            EntityReferences.animator.SetFloat(VelocityHash, MovementBehaviour.Velocity);
        }

        private void UpdateTileBelow()
        {
            MovementBehaviour.TileBelow = GetTileBelow();
            
            if (MovementBehaviour.TileBelow.hurtable)
                Hit(MovementBehaviour.TileBelow.damage);
        }

        private TileData GetTileBelow()
        {
            return TerrainHandler.GetTileAtPosition(transform.position);
        }
    }
}