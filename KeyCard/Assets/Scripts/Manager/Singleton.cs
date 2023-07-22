using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T _instance = null;
    static bool isDestroy = false;

    /// <summary>
    /// 싱글톤이 생성되어있는지 확인하고 생성되어있으면 리턴.
    /// 생성되어 있지 않으면 생성 후 리턴.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isDestroy)
                return null;
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    if (_instance == null)
                    {
                        // 인스턴스를 생성하지 못했을때 에러.
                        Debug.LogError("Singleton::Problem during the creation of " + typeof(T).ToString());
                    }
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    public static bool IsCreateInstance()
    {
        if (_instance == null)
            return false;
        return true;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            _instance.Init();
        }
    }

    /// <summary>
    /// 싱글톤이 생성될때.
    /// </summary>
    public virtual void Init() { }

    /// <summary>
    /// 싱글톤 해제될때.
    /// </summary>
    public virtual void Destory() { }

    void Release()
    {
        if (_instance != null)
        {
            //Debug.LogFormat("Singleton Release : {0}", name);
            _instance.Destory();
            Destroy(_instance.gameObject);
            _instance = null;
            //isDestroy = true;
        }
        //else
            //Debug.LogFormat("Singleton Release null : {0}", name);

    }

    /// <summary>
    /// GameObject 해제.
    /// </summary>
    void OnDestroy()
    {
        Release();
    }

    /// <summary>
    /// 어플리케이션 종료.
    /// </summary>
    void OnApplicationQuit()
    {
        //Release();
    }
}
