using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimerScript : MonoBehaviour
{
    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        int min = (GameData.Instance.seconds == 0 ? 10 : 9) - GameData.Instance.minutes;
        int sec = (GameData.Instance.seconds == 0 ? 0 : 60 - GameData.Instance.seconds);
        text.text = min + ":" + ((sec < 10) ? "0" + sec:""+sec);
    }
}
