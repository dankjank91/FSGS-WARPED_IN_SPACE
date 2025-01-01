using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CurvedText : Text
{
    public GameController manager;
    [Range(0.1f, 10.0f)]
    public float radius = 3.14f;

    [Range(0.1f, 360.0f)]
    public float wrapAngle = 360.0f;

    [Range(10.0f, 200.0f)]
    public float scaleFactor = 100.0f;

    private float _radius = -1;
    private float _scaleFactor = -1;
    private float _circumference = -1;
    float screenWidth = Screen.width / 20;
    float screenHeight = Screen.height / 20;
    float maxRadius;

    private float circumference
    {
        get
        {
            if (_radius != radius || _scaleFactor != scaleFactor)
            {
                _circumference = 2.0f * Mathf.PI * radius * 20;
                _radius = radius;
                _scaleFactor = scaleFactor;
            }
            return _circumference;
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        maxRadius = Mathf.Min(screenWidth, screenHeight) / 2.0f;
        if (radius <= 0.0f)
        {
            radius = 0.1f;
        }
        if (scaleFactor <= 0.0f)
        {
            scaleFactor = 10.0f;
        } 
        AdjustRadiusToFitScreen();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);

        List<UIVertex> stream = new List<UIVertex>();
        vh.GetUIVertexStream(stream);

        for (int i = 0; i < stream.Count; i++)
        {
            UIVertex v = stream[i];

            float percentCircumference = v.position.x / circumference;
            Vector3 offset = Quaternion.Euler(0.0f, 0.0f, -percentCircumference * wrapAngle) * Vector3.up;
            v.position = offset * radius * 20 + offset * v.position.y;
            v.position += Vector3.down * radius * 20;

            stream[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(stream);
    }

    void Update()
    {
        AdjustRadiusToFitScreen();
        if (radius <= 0.0f)
        {
            radius = 0.1f;
        }
        if (scaleFactor <= 0.0f)
        {
            scaleFactor = 20f;
        }

        rectTransform.sizeDelta = new Vector2(circumference * wrapAngle / 360.0f, rectTransform.sizeDelta.y);
    }

    private void AdjustRadiusToFitScreen()
    {
        float screenWidth = Screen.width / 20;
        float screenHeight = Screen.height / 20;
        float maxRadius = Mathf.Min(screenWidth, screenHeight) / 2.0f;
        radius = maxRadius;
    }
}