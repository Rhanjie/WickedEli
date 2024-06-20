using Map;
using UnityEngine;

namespace Entities.Characters.Interfaces
{
    public interface IMovementBehaviour
    {
        float Velocity { get; }
        TileData TileBelow { get; set; }

        void Move(Vector2 delta);
        void Stop();
    }
}