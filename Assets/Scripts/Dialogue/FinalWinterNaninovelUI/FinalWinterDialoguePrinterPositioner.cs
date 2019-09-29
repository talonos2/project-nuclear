using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWinterDialoguePrinterPositioner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RectTransform>().position = Camera.main.transform.position+new Vector3(0,0,1);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(.01f, .01f, .01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
