using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPortraitLoader : MonoBehaviour
{
    SpriteRenderer r;
    CharacterStats stats;

    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        r = GetComponent<SpriteRenderer>();
        r.sprite = stats.bustSprite;
        text.text = stats.charName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
