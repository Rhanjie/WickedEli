using UnityEngine;

public class CharacterSettings : ScriptableObject
{
    [Header("Main")]
    public string title;
    
    [Header("Attack")]
    public int health;
    public int damage;
    public float range;
    public float insensitivityTime;
    public float attackTime;
    public float nextAttackDelay;
    
    [Header("Movement")]
    public float speed;
    public float acceleration;
    public float friction;
}