using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning($"{typeof(T)} return null");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    if (m_Instance == null)
                    {
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + "(singleton)";
                        
                        DontDestroyOnLoad(singletonObject);
                    }
                }
            }

            return m_Instance;
        }
    }

    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
}