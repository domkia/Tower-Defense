using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            T other = FindObjectOfType<T>();
            if (instance != null && other != instance)
            {
                Debug.LogWarning("Found multiple singletons of type: " + typeof(T));
                Destroy(instance.gameObject);
            }
            else
                instance = other;
            return instance;
        }
    }
}