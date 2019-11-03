using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHPScript : MonoBehaviour
{
    TextMeshPro text;
    public Image bar;
    CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        text.renderer.sortingOrder = 50;
        bar.canvas.sortingOrder = 50;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = stats.HP + "/" + stats.MaxHP;
        bar.fillAmount = ((float)stats.HP / (float)stats.MaxHP);
    }
}
