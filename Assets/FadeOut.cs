using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    public float fadeTime = .125f;
    private float timeLeft;
    private string next;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = fadeTime;
    }

    public void InitNext(string next)
    {
        this.next = next;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", 1-timeLeft/fadeTime);
            Debug.Log("Last Plane: " + this.GetComponent<Renderer>().material.GetFloat("_Alpha"));
        }
        else
        {
            SceneManager.LoadScene(next);
        }
    }
}
