using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveStar : MonoBehaviour
{
    float speed { get; set; }
    Renderer renderer { get; set; }
    StarSpawner starGenerator;
    float timer;
    float timeout = .75f;
    public Camera mainCamera { get; private set; }
    Vector2 centerScreen = Vector2.zero;
    float distance;
    float maxDistance = 5f;
    float minSize = 0.09f;
    float maxSize = .3f;
    float minSpeed = 20;
    float maxSpeed = 80;
    Vector3 screenBounds;

    void Start()
    {
        starGenerator = GameObject.FindGameObjectWithTag("StarSpawner").GetComponent<StarSpawner>();
        if(renderer == null)
        {
        
            renderer = GetComponent<Renderer>();
        }


        speed = Random.Range(minSpeed, maxSpeed);

        float randomAngle = Random.Range(0f, 360f);
        float radians = Mathf.Deg2Rad * randomAngle;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        GetComponent<Rigidbody2D>().linearVelocity = (direction * speed);

        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            maxDistance = screenBounds.magnitude; // Use screen bounds for dynamic max distance
        }
    }

    void Update()
    {
        if (!renderer.isVisible)
        {
            starGenerator.starsInstatiated.Remove(this.gameObject);
            Destroy(this.gameObject, 0.2f);
        }
        CalculateSize();
    }

    void CalculateSize()
    {
        distance = Vector2.Distance(this.transform.position, centerScreen);
        if (maxDistance > 0)
        {
            float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
            float newScale = Mathf.Lerp(minSize, maxSize, normalizedDistance);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else
        {
            Debug.LogWarning("maxDistance is not set properly. Skipping size calculation.");
        }
    }
}
