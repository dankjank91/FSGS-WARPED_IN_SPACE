using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    public float speed;
    Renderer renderer;
    public Camera mainCamera { get; private set; }
    Vector2 centerScreen = new Vector2(0, 0);
    float distance;
    float maxDistance;
    public GameObject explosionOG;
    public GameObject explosionClone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        renderer = GetComponent<Renderer>();
        speed = Random.Range(2f, 4f);
        float randomAngle = Random.Range(0f, 360f);
        float radians = Mathf.Deg2Rad * randomAngle;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {       
            explosionClone = Instantiate(explosionOG, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(explosionClone, .65f);
            
        }
        else if(collision.gameObject.tag == "mainLaser")
        {
            Destroy(collision.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!renderer.isVisible)
        {
            Destroy(this.gameObject, 0.2f);
        }
        AsteroidFaceEdge();
    }
    void AsteroidFaceEdge()
    {
        Vector2 center = Vector2.zero;
        Vector2 asteroidPosition = transform.position;
        Vector2 directionToCenter = center - asteroidPosition;
        float angleToCenter = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(45f, 0f, angleToCenter);
    }
}
