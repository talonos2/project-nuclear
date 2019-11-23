using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortProperlyDarnit : MonoBehaviour
{
    public int sortingOrder;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().sortingOrder = sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
