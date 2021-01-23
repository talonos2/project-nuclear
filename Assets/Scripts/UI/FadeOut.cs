using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float fadeTime = .125f;
    private float timeLeft;
    private string next;
    private bool onGUI = false;
    private bool scenelessFadeoutFinished;
    private bool scenelessFadeout;

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
            if (!onGUI)
            {
                this.GetComponent<Renderer>().material.SetFloat("_Alpha", 1 - timeLeft / fadeTime);
            }
            else
            {
                this.GetComponent<Image>().color = new Color(0,0,0, 1 - timeLeft / fadeTime);
            }
        }
        else
        {
            if (!scenelessFadeout) SceneManager.LoadScene(next);
        }
    }

    internal bool scenelessFadeOutFinished()
    {
        return scenelessFadeoutFinished;
    }

    internal void InitScenelessFadeout(float fadeTime)
    {
        this.fadeTime = fadeTime;
        scenelessFadeout = true;
    }

    internal void attachToGUI(Canvas canvas)
    {
        onGUI = true;
        RectTransform rt = this.gameObject.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform);
        rt.localScale = new Vector3(500, 500, 1);
        GameObject.Destroy(this.GetComponent<Renderer>());
        Image i = this.gameObject.AddComponent<Image>();
        i.color = new Color(0f, 0f, 0f, 0f);
    }
}
