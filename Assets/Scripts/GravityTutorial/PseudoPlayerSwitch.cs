using System.Collections.Generic;
using UnityEngine;

public class PseudoPlayerSwitch : MonoBehaviour
{
    public List<GameObject> objectsToDestroy;
    public List<GameObject> objectsToActivate;
    
    void Start()
    {
        
    }

    void Update()
    {
    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            if (objectsToDestroy != null)
            {
                foreach (GameObject obj in objectsToDestroy)
                {
                    Destroy(obj);
                }
            }
            if (objectsToActivate != null)
            {
                foreach (GameObject obj in objectsToActivate)
                {
                    obj.SetActive(true);
                } 
            }
        }
    }
}
