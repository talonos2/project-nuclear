using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInitiator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

            Debug.Log("entered");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("huh");
    }
}
