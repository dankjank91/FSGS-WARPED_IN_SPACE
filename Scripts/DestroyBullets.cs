using UnityEngine;

public class DestroyBullets : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "mainLaser")
        {
            Destroy(collision.gameObject);
        }
    }
}
