using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// 
/// If you use Awake u must write   public override void Awake(){base.Awake() /your code here/ }
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T) FindObjectOfType(typeof (T));

                    if (FindObjectsOfType(typeof (T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
                        return _instance;
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (FindObjectsOfType(typeof (T)).Length > 1)
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}