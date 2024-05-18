using UnityEngine;

namespace Characters.Interfaces
{
    public interface IMovementBehaviour
    {
        float Velocity { get; }
        
        void Move(Vector2 delta);
        void Stop();
    }
}