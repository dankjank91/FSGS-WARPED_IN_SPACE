using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class StarSpawner : MonoBehaviour
{
    public GameController Controller;
    public static StarSpawner Instance;
    public Camera mainCamera {  get; private set; }
    public Vector3 screenBounds {  get; private set; }
    public GameObject starObject;
    Vector2 centerScreen = new Vector2(0, 0);
    [SerializeField]
    private int maxStars = 50;
    private float timer { get; set; }
    public float maxTime = 1;
    public List<GameObject> starsInstatiated;


    void Awake()
    {
        if (Controller == null)
        {
            Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (starObject == null)
        {
            starObject = GameObject.FindGameObjectWithTag("Star");
            mainCamera = Camera.main;
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        }
        

    }

    private void Update()
    {
        if (Controller.state == "levelrunning")
        {
            timer += Time.deltaTime;
            if (timer >= maxTime && starsInstatiated.Count <= maxStars)
            {
                CreateStar();
                timer = 0;
            }
        }

    }

    void CreateStar()
    {

        for (int i = 0; i < maxStars; i++)
        {
            float randomSize = Random.Range(0.01f, .06f);
            GameObject newStar = Instantiate(starObject);
            newStar.transform.position = centerScreen;
            starsInstatiated.Add(newStar);
            newStar.transform.localScale = new Vector3(randomSize, randomSize, 0);

        }


    }
}
