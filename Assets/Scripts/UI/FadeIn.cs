using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float fadeTime = .125f;
    public bool celesteStyle = false;
    public Vector2 fadeInWorldLocation = new Vector2(0, 0);
    private float defaultFadeTime = .125f;
    private float timeLeft;
    private Renderer meshRenderer;
    private bool shortCutFadeIn;
    private bool shortCutFadeOut;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = fadeTime;
        meshRenderer = this.GetComponent<Renderer>();
        meshRenderer.enabled = true;
        meshRenderer.material.SetFloat("_Alpha", 1);
    }

    public void enableShortcutFadeIn(float fadeDuration)
    {
        if (meshRenderer==null) meshRenderer = this.GetComponent<Renderer>();
        meshRenderer.enabled = true;
        fadeTime = fadeDuration;
        timeLeft = fadeDuration;
        meshRenderer.material.SetFloat("_Alpha", 1);
        shortCutFadeIn = true;
        shortCutFadeOut = false;
    }
    public void enableShortcutFadeOut(float fadeDuration)
    {
        if (meshRenderer == null) meshRenderer = this.GetComponent<Renderer>();
        meshRenderer.enabled = true;
        fadeTime = fadeDuration;
        timeLeft = fadeDuration;
        meshRenderer.material.SetFloat("_Alpha", 0);
        shortCutFadeOut = true;
        shortCutFadeIn = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (!meshRenderer.enabled) return;

        if (shortCutFadeIn)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0)
            {
                meshRenderer.material.SetFloat("_Alpha", timeLeft / fadeTime);
            }
            else
            {
                //Shortcut controls when to unpause or not
                meshRenderer.enabled = false;
                shortCutFadeIn = false;
                fadeTime = defaultFadeTime;//setting fade time back to default
            }
        }
        else if (shortCutFadeOut)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0)
            {

                meshRenderer.material.SetFloat("_Alpha", 1 - timeLeft / fadeTime);
            }
            else
            {
                //Shortcut controls when to unpause or not
                meshRenderer.enabled = false;
                shortCutFadeOut = false;
                fadeTime = defaultFadeTime;//setting fade time back to default
            }

        }
        else {
            timeLeft -= Time.deltaTime;
            if (timeLeft > 0)
            {
                if (celesteStyle)
                {
                    meshRenderer.material.SetVector("_Location", fadeInWorldLocation);
                    meshRenderer.material.SetFloat("_Iris", timeLeft / fadeTime);
                }
                else
                {
                    meshRenderer.material.SetFloat("_Alpha", timeLeft / fadeTime);
                }
            }
            else
            {
                GameState.fullPause = false;
                //Debug.Log("Fade-in complete! If things are still frozen, it's somebody else's fault now. :P");
                //GameObject.Destroy(this.gameObject);
                meshRenderer.enabled = false;
            }

        }
       

    }
}
