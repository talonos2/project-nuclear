using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float fadeTime = .125f;
    private float timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = fadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", timeLeft/fadeTime);
            Debug.Log("Amount through:" + (timeLeft/fadeTime));
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
