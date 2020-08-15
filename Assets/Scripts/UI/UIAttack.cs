using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAttack : MonoBehaviour
{
    TextMeshPro text;
    CharacterStats stats;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + stats.attack;
    }
}
