using UnityEngine;

namespace Characters.Settings
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "Settings/Enemy")]
    public class EnemySettings : CharacterSettings
    {
        public int experience;
    }
}