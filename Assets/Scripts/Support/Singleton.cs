using UnityEngine;


/// <summary>
/// Normal singleton base class
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : class, new()
{

    protected static T _instance = null;

    /// <summary>
    /// Whether or not the singleton instance exists
    /// </summary>
    public static bool HasInstance
    {
        get { return (_instance != null); }
    }

    /// <summary>
    /// The singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}

/// <summary>
///  Singleton base class for MonoBehaviours
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;

    /// <summary>
    /// The singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Whether or not the singleton instance exists
    /// </summary>
    public static bool HasInstance
    {
        get { return (_instance != null); }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.Log("Destroying additional SingletonBehaviour: " + typeof(T).Name + " in " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            _instance = null;
        }
    }
}

/// <summary>
///  Singleton base class for ScriptableObject, an instance of this scriptable object must exist in the resources folder
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    protected static T _instance = null;

    /// <summary>
    /// Whether or not the singleton instance exists
    /// </summary>
    public static bool HasInstance
    {
        get { return (_instance != null); }
    }

    /// <summary>
    /// The singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = Resources.Load(typeof(T).Name, typeof(T)) as T;

            if (_instance == null)
            {
                Debug.LogError("Could not load ScriptableObjectSingleton: " + typeof(T) + " - Make sure it exists in the Resources directory.");
            }

            return _instance;
        }
    }
}


/// <summary>
/// A singleton behaviour that will create itself if it doesn't already exist
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AutoSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;

    /// <summary>
    /// The singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null && (!Application.isEditor || Application.isPlaying))
            {
                CreateInstance();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Whether or not the singleton instance exists
    /// </summary>
    public static bool HasInstance
    {
        get { return (_instance != null); }
    }

    /// <summary>
    /// Creates the singleton instance if it does not already exist.
    /// </summary>
    public static void EnsureInstanceExists()
    {
        if (_instance == null && (!Application.isEditor || Application.isPlaying))
        {
            CreateInstance();
        }
    }

    static void CreateInstance()
    {
        GameObject singletonParent = GameObject.Find("_Singletons");
        if (singletonParent == null)
        {
            singletonParent = new GameObject("_Singletons");
            DontDestroyOnLoad(singletonParent);
        }
        GameObject newGo = new GameObject(typeof(T).ToString());
        newGo.transform.parent = singletonParent.transform;
        _instance = newGo.AddComponent<T>();
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Debug.Log("Destroying additional SingletonBehaviour: " + typeof(T));
            Destroy(gameObject);
        }
    }

    public virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}

/// <summary>
/// A singleton behaviour that will instantiate itself from a prefab in the resources directory if it doesn't already exist
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AutoSingletonPrefab<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;

    /// <summary>
    /// The singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null && (!Application.isEditor || Application.isPlaying))
            {
                CreateInstance();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Whether or not the singleton instance exists
    /// </summary>
    public static bool HasInstance
    {
        get { return (_instance != null); }
    }

    /// <summary>
    /// Creates the singleton instance if it does not already exist.
    /// </summary>
    public static void EnsureInstanceExists()
    {
        if (_instance == null && (!Application.isEditor || Application.isPlaying))
        {
            CreateInstance();
        }
    }

    static void CreateInstance()
    {

        GameObject singletonParent = GameObject.Find("_Singletons");
        if (singletonParent == null)
        {
            singletonParent = new GameObject("_Singletons");
            DontDestroyOnLoad(singletonParent);
        }

        GameObject prefab = Resources.Load<GameObject>(typeof(T).Name);

        if (prefab == null)
        {
            Debug.LogError("Could not load AutoSingletonPrefab: " + typeof(T) + " - Make sure it exists in the Resources directory.");
        }

        _instance = Instantiate(prefab).GetComponent<T>();

        if (_instance == null)
        {
            Debug.LogError("Could not load AutoSingletonPrefab: " + typeof(T) + " - Prefab does not have the component.");
        }

        _instance.gameObject.name = typeof(T).ToString();
        _instance.transform.parent = singletonParent.transform;

    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.Log("Destroying additional SingletonBehaviour: " + typeof(T));
            Destroy(gameObject);
        }
    }

    public virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}