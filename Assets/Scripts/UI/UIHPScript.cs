using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHPScript : MonoBehaviour
{
    TextMeshPro text;
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
        text.text = stats.HP + "/" + stats.MaxHP;
    }
}
