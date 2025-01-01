using Unity.VisualScripting;
using UnityEngine;

public class MovePlayerLaser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.Lerp(this.transform.position, new Vector2(0,0),6*Time.deltaTime);
        if(this.transform.position == Vector3.zero)
        {
            Destroy(this.gameObject);
        }
      
    }

}
