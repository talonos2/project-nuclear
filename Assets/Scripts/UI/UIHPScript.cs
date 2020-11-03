using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHPScript : MonoBehaviour
{
    private float timeSoFar;
    private int lastBeepAt;

    public float beepFrequency = .8f;

    TextMeshPro text;
    public Image bar;
    public SpriteRenderer backgroundBlobToFlash;
    CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSoFar += Time.deltaTime;

        text.text = stats.HP + "/" + stats.MaxHP;
        bar.fillAmount = ((float)stats.HP / (float)stats.MaxHP);
        if (stats.HP <= stats.MaxHP / 5)
        {
            float t = Mathf.Sin(timeSoFar*4) / 2 + .5f;
            bar.color = new Color(1, t, t);
            backgroundBlobToFlash.color = new Color((1-t)/3, 0, 0);
            int beepNum = (int)(timeSoFar / beepFrequency);
            if (lastBeepAt == beepNum-1)
            {
                SoundManager.Instance.PlaySound("Old/MenuMoveOld", 1.0f);
            }
            lastBeepAt = beepNum;
        }
        else
        {
            bar.color = Color.white;
            backgroundBlobToFlash.color = Color.black;
        }
    }
}
