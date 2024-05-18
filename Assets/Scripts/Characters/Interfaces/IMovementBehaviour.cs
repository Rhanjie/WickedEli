using UnityEngine;

namespace Characters.Interfaces
{
    public interface IMovementBehaviour
    {
        void Move(Vector2 delta);
        void Stop();
    }
}