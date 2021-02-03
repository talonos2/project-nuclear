using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInUISpace : MonoBehaviour
{
    public float fadeTime = .125f;

    private float timeLeft;
    private RawImage image;
    private bool shortCutFadeIn;
    private bool shortCutFadeOut;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = fadeTime;
        image = this.GetComponent<RawImage>();
        image.enabled = true;
        image.color = new Color(0,0,0,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!image.enabled) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            image.color = new Color(0, 0, 0, timeLeft/fadeTime);
        }
        else
        {
            image.enabled = false;
            GameState.setFadeinPause(false);
        }
    }
}
