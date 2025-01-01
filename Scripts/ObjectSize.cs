using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSize : MonoBehaviour
{
    public Camera mainCamera { get; private set; }
    Vector2 centerScreen = new Vector2(0, 0);
    float distance;
    float maxDistance;
    float minSize = 0.01f;
    float maxSize = 7f;
    Vector3 screenBounds;

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        maxDistance = Vector2.Distance(centerScreen, new Vector2(screenBounds.x, screenBounds.y));
    }

    void Update()
    {
        CalculateSize();
    }

    void CalculateSize()
    {
        distance = Vector2.Distance(this.transform.position, centerScreen);
        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        float newScale = Mathf.Lerp(minSize, maxSize, normalizedDistance);
        transform.localScale = new Vector3(newScale, newScale, newScale);

        //Debug.Log($"Distance = {distance}, Normalized Distance = {normalizedDistance}, New Scale = {newScale}");
    }
}
