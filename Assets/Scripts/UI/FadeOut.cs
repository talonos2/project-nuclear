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
        GameState.fullPause = true;
        this.GetComponent<Renderer>().material.SetFloat("_Alpha", 0);
       // Debug.Log("Is everything frozen? It's probably because you did a fade-out without a fade-in afterwards.");
        timeLeft = fadeTime;
    }

    public void InitNext(string next)
    {
        this.next = next;
    }

    public void InitNext(string next, float fadeTime)
    {
        this.next = next;
        this.fadeTime = fadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", 1-timeLeft/fadeTime);
        }
        else
        {
            SceneManager.LoadScene(next);
        }
    }
}
