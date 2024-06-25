using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities.Characters.Players;
using Map;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Entities.Characters.Submodules
{
    public class Spawner : MonoBehaviour
    {
        private StaticEntity _entity;
        private Settings _settings;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(StaticEntity entity, Settings settings, DiContainer diContainer)
        {
            _entity = entity;
            _settings = settings;
            _diContainer = diContainer;

            Timing.RunCoroutine(SpawnBehaviour());
        }

        private IEnumerator<float> SpawnBehaviour()
        {
            while (true)
            {
                var random = Random.Range(_settings.timeBetweenSpawns.x, _settings.timeBetweenSpawns.y);
                yield return Timing.WaitForSeconds(random);

                if (!IsPlayerNearby())
                    continue;
                
                var chosenCharacterSettings = TerrainGenerator.GetRandomVariant(_settings.generables);
                if (chosenCharacterSettings != null)
                    GenerateObject(chosenCharacterSettings, transform.position);
            }
        }

        private bool IsPlayerNearby()
        {;
            //TODO: Temporary fix. Sometimes spawner dependencies got inject before entity
            var visionRange = 50f;

            if (_entity.EntitySettings != null)
                visionRange = _entity.EntitySettings.visionRange;
            
            //TODO: Temporary solution
            var layerMask = LayerMask.GetMask("Player");
            var result = Physics2D.OverlapCircle(
                transform.position, visionRange, layerMask
            );

            var player = result != null ? result.GetComponent<Player>() : null;
            return player != null;
        }
        
        private void GenerateObject(CharacterSettingsInstaller characterSettings, Vector3 position)
        {
            var context = _settings.characterPrefab.GetComponent<GameObjectContext>();
            context.ScriptableObjectInstallers = new List<ScriptableObjectInstaller> { characterSettings };

            var entity = _diContainer.InstantiatePrefab(
                _settings.characterPrefab, position, Quaternion.identity, null
            );
        }

        [Serializable]
        public class Settings
        {
            [MinMaxSlider(1, 10)]
            public Vector2 timeBetweenSpawns;

            public float spawnRange;

            public Transform characterPrefab;
        
            [AssetsOnly]
            public List<Generable<CharacterSettingsInstaller>> generables;
        }
    }
}
