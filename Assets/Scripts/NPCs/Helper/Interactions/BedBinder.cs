using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBinder : MonoBehaviour
{
    public GameObject bedRefGO = null;

    // Start is called before the first frame update
    void Start()
    {
        if (bedRefGO == null)
        {
            this.gameObject.GetComponent<BedBinder>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
