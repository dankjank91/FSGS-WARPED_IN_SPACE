using System.Collections;
using UnityEngine;

public class PlayerCannons : MonoBehaviour
{
    public GameObject mainLaserBeam;
    public GameObject firedLaser;
    public GameObject dualLaserA, dualLaserB;
    public GameObject secondaryLaserBeam;
    public GameObject[] dualLaserBeams;
    public GameObject currentLaserBeam;
    public bool fire = false;
    public bool dualBeams = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FireMainLaser());
    }

    IEnumerator FireMainLaser()
    {            

        if (fire&&!dualBeams)
        {
            yield return new WaitForEndOfFrame();
            firedLaser = Instantiate(mainLaserBeam,this.transform.position,Quaternion.identity);      
            fire = false;
        }
        else if(fire && dualBeams)
        {
            dualLaserA = Instantiate(mainLaserBeam,new Vector3(this.transform.position.x+.1f,this.transform.position.y),Quaternion.identity);
            dualLaserB = Instantiate(mainLaserBeam,new Vector3(this.transform.position.x-.1f,this.transform.position.y),Quaternion.identity);
            fire = false;
        }
        


    }
}
