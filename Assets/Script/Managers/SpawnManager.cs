using PoolSystem;
using FullSerializer;
using GameCore;
using System.IO;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public Transform parent;
        public Level currentLevel;
      
        public GameObject Spawn(Vector3 pos, string spawnName)
        {
            var fx = ObjectPooler.Instance.Spawn(spawnName, new Vector3());
            fx.transform.position = pos;
            return fx;
        }

        public GameObject Spawn(Transform parent, string spawnName)
        {
            var fx = ObjectPooler.Instance.Spawn(spawnName, new Vector3());
            fx.transform.position = parent.position;
            fx.transform.SetParent(parent);
            return fx;
        }

        [ContextMenu("Spawn TestObj")]
        public void SpawntObj()
        {
            float baseValue = currentLevel.width / 2 * 0.25f;
            for (var j = 0; j < currentLevel.height; j++)
            {
                for (var i = 0; i < currentLevel.width; i++)
                {
                    var idx = i + (j * currentLevel.width);
                    Spawn(new Vector3(i * 0.25f - baseValue, 0, j * 0.25f), "Cube" + currentLevel.tiles[idx].color);
                }
            }
        }

        protected T LoadJsonFile<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var file = new StreamReader(path);
            var fileContents = file.ReadToEnd();
            var data = fsJsonParser.Parse(fileContents);
            object deserialized = null;
            var serializer = new fsSerializer();
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            file.Close();
            return deserialized as T;
        }
    }
}