using UnityEngine;
using System.IO;
using System;
using Unity.Plastic.Newtonsoft.Json;
using Utils.ServiceLocator;

namespace SiliconeHeart.Save
{
    public class JsonToFileStorageService : IStorage
    {
        public void Save(string key, object value, Action<bool> callback = null)
        {
            string filePath = BuildPath(key);
            string json = JsonConvert.SerializeObject(value);

            using (var fileStream = new StreamWriter(filePath))
            {
                fileStream.Write(json);
            }

            callback?.Invoke(true);
        }

        /// <summary>
        /// If the file by the key was not found, the callback will not be called.
        /// </summary>
        public void Load<T>(string key, Action<T> callback)
        {
            string filePath = BuildPath(key);

            if (File.Exists(filePath))
            {
                using (var fileStream = new StreamReader(filePath))
                {
                    var json = fileStream.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<T>(json);

                    callback.Invoke(data);
                }
            }
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}
