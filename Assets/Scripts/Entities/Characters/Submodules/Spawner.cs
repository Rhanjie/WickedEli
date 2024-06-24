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
        private Settings _settings;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(Settings settings, DiContainer diContainer)
        {
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

                var chosenCharacterSettings = TerrainGenerator.GetRandomVariant(_settings.generables);
                if (chosenCharacterSettings != null)
                    GenerateObject(chosenCharacterSettings, transform.position, null);
            }
        }
        
        private void GenerateObject(CharacterSettingsInstaller characterSettings, Vector3 position, Transform parent)
        {
            var context = _settings.characterPrefab.GetComponent<GameObjectContext>();
            context.ScriptableObjectInstallers = new List<ScriptableObjectInstaller> { characterSettings };

            var entity = _diContainer.InstantiatePrefab(
                _settings.characterPrefab, position, Quaternion.identity, parent
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
