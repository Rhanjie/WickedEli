using UnityEngine;

interface IHittable
{
    public Transform Handler { get; }

    public void Hit(int damage);
}