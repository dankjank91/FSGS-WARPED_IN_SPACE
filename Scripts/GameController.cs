using UnityEngine;
using STATEMACHINE;
using System.Collections.Generic;

public class GameController : StateMachine
{
    public static GameController Instance { get; private set; }
    Camera mainCamera;
    [SerializeField]
    bool stateflag = false;
    public int numberOfEnemies = 5;
    public int numberOfWaves = 3;
    public float waveSpawnInterval = 3;
    public float enemyMoveInterval = .25f;
    public float enemyMinSpeed = 3;
    public float enemyMaxSpeed = 5;
    public float playerSpeed = 5;
    public float enemyProjectileSpeed = 5;
    public float playerProjectileSpeed = 5;
    [SerializeField]
    int currentWave { get; set; }
    [SerializeField]
    int currentLevel { get; set; }
    public List<GameObject> gameTextObjects;
    public List<GameObject> gameOverTextObjects;
    public List<GameObject> gameWinTextObjects;
    public List<GameObject> gameMenuTextObjects;
    public List<GameObject> gamePauseTextObjects;
    public List<GameObject> enemysPrefabs;
    public List<GameObject> powerUpPrefabs;
    public List<GameObject> playerPrefabs;
    public List<GameObject> projectilePrefabs;
    public List<GameObject> asteroidObjectsPrefabs;
    public enum Edges { Left, Right, Top, Bottom }
    private string[] edges = new string[4];
    private List<float> floats = new List<float>();
    public float radius { get; private set; }
    public List<Vector3> screenEdges = new List<Vector3>();
    public Vector3 screenBounds {  get; private set; }
    public Vector3 playerStartPosition { get; private set; }
    public Vector3 playerRespawnPosition { get; private set; }
    public List<List<GameObject>> clonedEnemies = new List<List<GameObject>>();
    public List<List<GameObject>> clonedPowerUps = new List<List<GameObject>>();
    public List<List<GameObject>> clonedProjectiles = new List<List<GameObject>>();
    public List<List<GameObject>> clonedAsteroidObjects = new List<List<GameObject>>();
    public List<List<Vector3>> waypoints = new List<List<Vector3>>();
    GameObject playerClone { get; set; }
    public List<List<bool>> enemySlotsAvailable = new List<List<bool>>();
    public List<LineRenderer> pathRenderers = new List<LineRenderer>();
    public List<List<LineRenderer>> renderedWavePaths = new List<List<LineRenderer>>();
    public LineRenderer lineEdgeRenderer;
    [SerializeField]
    bool waveReady { get; set; }
    [SerializeField]
    bool levelCleared { get; set; }
    public int currentShip = 0;
    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }

        Instance = this;

        // Mark this object as not to be destroyed on scene load
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
       
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        set_state("title_screen");
        SetEdges();
    }
    public override void _state_logic()
    {
        StateController(state);
    }
    string StateController(string state)
    {
        // stateflag>break statement will run 60xSecond, the state runs here because it wasn't changed yet.
        // outside of the stateflag>break will run only if stateflag was set to false and only once per state change.
        switch (state)
        {
            case "title_screen":
                if (stateflag)
                {
                    if (Input.anyKeyDown)
                    {
                        set_state("createlevel");
                        stateflag = false;
                    }
                    break;
                }
                stateflag = true;
                Debug.Log($"State transitioned to {state}");
                return transition;
            case "createlevel":
                if (stateflag)
                {
                    break;
                }
                stateflag = true;
                SwitchTitleScreen(false);
                CreatePlayerRadius();
                set_state("createwave");
                stateflag = false;
                Debug.Log($"State transitioned to {state}");
                return transition;
            case "createwave":
                if (stateflag)
                {
                    break;
                }
                stateflag = true;
                CreateEnemySlots(numberOfWaves,numberOfEnemies);
                Debug.Log($"State transitioned to {state}");
                if(enemySlotsAvailable.Count != 0)
                {
                    set_state("levelrunning");
                    stateflag = false;
                }

                return transition;
            case "levelrunning":
                if (stateflag)
                {
                    break;
                }
                SwitchGameBoard(true);
                SetPlayer(currentShip);
                stateflag = true;
                Debug.Log($"State transitioned to {state}");
                return transition;
            case "gamepaused":
                if (stateflag)
                {
                    break;
                }
                stateflag = true;
                Debug.Log($"State transitioned to {state}");
                return transition;
            case "playerdied":
                if (stateflag)
                {
                    break;
                }
                stateflag = true;
                Debug.Log($"State transitioned to {state}");
                return transition;
            case "gameover":
                if (stateflag)
                {
                    break;
                }
                stateflag = true;
                Debug.Log($"State transitioned to {state}");
                return transition;
        }
        return transition;
    }
    void SetEdges()
    {
        for (int i = 0; i < edges.Length; i++)
        {
            switch (i)
            {
                case 0:
                    edges[i] = Edges.Left.ToString();
                    break;
                case 1:
                    edges[i] = Edges.Right.ToString();
                    break;
                case 2:
                    edges[i] = Edges.Top.ToString();
                    break;
                case 3:
                    edges[i] = Edges.Bottom.ToString();
                    break;
                default:
                    Debug.Log("There were no matching edges.");
                    break;
            }
        }
        for (int i = 0; (i < edges.Length); i++)
        {
            floats.Add(GetScreenBounds(edges[i]));
        }
        for(int i = 0; i < floats.Count; i++)
        {
            switch (i)
            {
                case 0:
                    screenEdges.Add(new Vector3(0, floats[i], 0));
                    break;
                case 1:
                    screenEdges.Add(new Vector3(0, floats[i], 0));
                    break;
                case 2:
                    screenEdges.Add(new Vector3(floats[i],0, 0));
                    break;
                case 3:
                    screenEdges.Add(new Vector3(floats[i], 0, 0));
                    break;
                default:
                    break;
            }
        }
        SetBoundaries();
    }
    void SetBoundaries()
    {
        Camera mainCamera = Camera.main;
        lineEdgeRenderer = this.GetComponent<LineRenderer>();
        lineEdgeRenderer.loop = true;
        lineEdgeRenderer.positionCount = edges.Length;
        lineEdgeRenderer.startWidth = 0.1f;
        lineEdgeRenderer.endWidth = 0.1f;
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
        Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));
        Vector3 bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        for (int i = 0; i < edges.Length; i++)
        {
            switch (i)
            {
                case 0:
                    lineEdgeRenderer.SetPosition(i, topRight);
                    break;
                case 1:
                    lineEdgeRenderer.SetPosition(i, topLeft);
                    break;
                case 2:
                    lineEdgeRenderer.SetPosition(i, bottomLeft);
                    break;
                case 3:
                    lineEdgeRenderer.SetPosition(i, bottomRight);
                    break;
                default: break;
            }

        }
    }
    public float GetScreenBounds(string ScreenEdge)
    {
        Camera mainCamera = Camera.main;
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
        float left = bottomLeft.x;
        float right = topRight.x;
        float top = topRight.y;
        float bottom = bottomLeft.y;
        float edge = 0;
        //Debug.Log($"Screen Edges - Left: {left}, Right: {right}, Top: {top}, Bottom: {bottom}");
        switch (ScreenEdge.ToLower())
        {
            case "left":
                edge = left;
                return edge;
            case "right":
                edge = right;
                return edge;
            case "top":
                edge = top;
                return edge;
            case "bottom":
                edge = bottom;
                return edge;

        }
        return edge;
    }
    void SetPlayer(int value)
    {
        playerStartPosition = new Vector3(0, -screenBounds.y - 5);
        if(playerClone == null)
        {
            playerClone = Instantiate(playerPrefabs[value],playerStartPosition,Quaternion.identity);
        }
        Debug.Log("Player created, set to offscreen starting position");
    }
    void CreatePlayerRadius()
    {
        radius = screenBounds.y - 6f;
        LineRenderer playerRails = new GameObject("PlayerRails").AddComponent<LineRenderer>();
        playerRails.gameObject.tag = "PlayerRails";
        playerRails.positionCount = 360;
        playerRails.loop = true;
        playerRails.startWidth = .0f;
        playerRails.endWidth = .0f;
        for (int i = 0; i < playerRails.positionCount; i++)
        {
            float angle = (float)i / playerRails.positionCount * 2 * Mathf.PI;
            Vector2 position = new Vector2(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius);
            playerRails.SetPosition(i, position);  
        }
        playerRespawnPosition = playerRails.GetPosition(270);
        Debug.Log($"Player movement rails were generated. Respawn position set to {270}:{playerRespawnPosition}");
    }
    void CreateEnemySlots(int waves,int enemies)
    {
        for (int i = 0; i < waves; i++)
        {
            enemySlotsAvailable.Add(new List<bool>());
            for (int j = 0; j < enemies; j++)
            {
                enemySlotsAvailable[i].Add(true);
            }
            if (currentWave <= waves)
            {
                currentWave++;
            }
        }
        Debug.Log($"Enemy slots created, can hold up to {enemySlotsAvailable.Count} waves of {enemySlotsAvailable[0].Count} enemies each while the slot is marked true.");
    }
    void SwitchTitleScreen(bool tf)
    {
        for (int i = 0; i < gameMenuTextObjects.Count; i++)
        {
            gameMenuTextObjects[i].gameObject.SetActive(tf);
        }
    }

    void SwitchGameBoard(bool tf)
    {
        for (int i = 0;i < gameTextObjects.Count; i++)
        {
            gameTextObjects[i].gameObject.SetActive(tf);
        }
    }

    //void ServeClouds(int numwaves, int numclouds)
    //{
    //    for (int i =0; i < numwaves; i++)
    //    {
    //        clonedCloudObjects.Add(new List<GameObject>());
    //        for (int j = 0; j < numclouds; j++)
    //        {
    //            int modulate = j % enemySlotsAvailable[i].Count;
    //            if (enemySlotsAvailable[i][modulate])
    //            {
    //                int randomCloud = Random.Range(0, cloudObjectsPrefabs.Count);
    //                GameObject clone = Instantiate(cloudObjectsPrefabs[randomCloud].gameObject, new Vector3(Random.Range(GetScreenBounds("left"),GetScreenBounds("right")), Random.Range(GetScreenBounds("top"), GetScreenBounds("bottom"))),Quaternion.identity);
    //                clonedCloudObjects[i].Add(clone);   
    //                enemySlotsAvailable[i][modulate] = false;
    //            }
    //        }
    //    }
    //}
}
