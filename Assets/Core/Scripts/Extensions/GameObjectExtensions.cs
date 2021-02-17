using UnityEngine;

namespace Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static T CreateNew<T>(this GameObject gameObject, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            var result = parent == null
                ? Object.Instantiate(gameObject)
                : Object.Instantiate(gameObject, parent, instantiateInWorldSpace);

            return result.GetComponent<T>();
        }
    }
}
