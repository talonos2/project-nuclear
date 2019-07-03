using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public GameObject[] fireworks;
    int fireworkType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Random.insideUnitCircle*8;
            GameObject.Instantiate(fireworks[(fireworkType++)%fireworks.Length], pos, Quaternion.Euler(0,0,1));
        }
    }
}
