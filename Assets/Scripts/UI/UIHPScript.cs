using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHPScript : MonoBehaviour
{
    private float timeSoFar;
    private int lastBeepAt;

    private int oldHP;
    private float timeSinceFlash = 100f;

    public float beepFrequency = .8f;

    TextMeshPro text;
    public Image bar;
    public Image flashBar;
    public SpriteRenderer backgroundBlobToFlash;
    CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        oldHP = stats.HP;
    }

    // Update is called once per frame
    void Update()
    {
        timeSoFar += Time.deltaTime;

        text.text = stats.HP + "/" + stats.MaxHP;
        bar.fillAmount = ((float)stats.HP / (float)stats.MaxHP);
        flashBar.fillAmount = ((float)stats.HP / (float)stats.MaxHP);

        if (stats.HP>oldHP+stats.MaxHP*.1f|| (oldHP != stats.MaxHP&&stats.HP==stats.MaxHP))
        {
            //Debug.Log("Constant: " + oldHP + ", " + (stats.HP + stats.MaxHP * .1f) + ", " + (oldHP != stats.MaxHP) + ", " + (stats.HP == stats.MaxHP));
            timeSinceFlash = 0;
        }

        timeSinceFlash += Time.deltaTime;

        oldHP = stats.HP;


        if (timeSinceFlash < .5f)
        {
            bar.color = Color.white;
            backgroundBlobToFlash.color = Color.black;
            flashBar.color = new Color(1, 1, 1, Mathf.Pow(1 - (timeSinceFlash * 2), 2));
        }
        else if (stats.HP <= stats.MaxHP / 5)
        {
            float t = Mathf.Sin(timeSoFar*4) / 2 + .5f;
            bar.color = new Color(1, t, t);
            flashBar.color = new Color(1, 1, 1, 0);
            backgroundBlobToFlash.color = new Color((1-t)/3, 0, 0);
            int beepNum = (int)(timeSoFar / beepFrequency);
            if (lastBeepAt == beepNum-1)
            {
                SoundManager.Instance.PlaySound("Old/MenuMoveOld2", 1.0f);
            }
            lastBeepAt = beepNum;
        }
        else
        {
            bar.color = Color.white;
            backgroundBlobToFlash.color = Color.black;
            flashBar.color = new Color(1, 1, 1, 0);
        }
    }
}
