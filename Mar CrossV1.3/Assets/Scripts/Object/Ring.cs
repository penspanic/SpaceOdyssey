using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour
{
    public PlanetBase targetPlanet;

    void Awake()
    {

    }
    public bool isChasing = false;
    void Update()
    {
        if(isChasing)
        {
            if(this.transform.position.x<=-6.4f)
            {
                Destroy(gameObject);
                return;
            }
            this.transform.position = targetPlanet.transform.position;
        }
    }
}
