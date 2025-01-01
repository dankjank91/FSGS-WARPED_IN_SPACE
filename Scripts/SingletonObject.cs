using UnityEngine;

public class SingletonObject : MonoBehaviour
{
    public static SingletonObject Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
