using System;
using Utils.ServiceLocator;

namespace SiliconeHeart.Save
{
    public interface IStorage : IService
    {
        void Save(string key, object value, Action<bool> callback = null);
        void Load<T>(string key, Action<T> callback);
    }
}
