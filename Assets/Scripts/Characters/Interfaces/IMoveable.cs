using UnityEngine;

namespace Characters.Interfaces
{
    public interface IMoveable
    {
        void Move(Vector2 delta);
        void Stop();
    }
}