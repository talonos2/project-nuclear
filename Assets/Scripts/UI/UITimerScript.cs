using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimerScript : MonoBehaviour
{
    int oldSec;
    TextMeshPro text;
    TextMeshPro pulseText;

    float pulse = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        pulseText = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        int min = (GameData.Instance.seconds == 0 ? 10 : 9) - GameData.Instance.minutes;
        int sec = (GameData.Instance.seconds == 0 ? 0 : 60 - GameData.Instance.seconds);
        if (min == 0)
        {
            CheckForThump(sec);
        }
        oldSec = sec;
        text.text = min + ":" + ((sec < 10) ? "0" + sec:""+sec);

        if (pulse > 0)
        {
            pulse = Mathf.Clamp01(pulse - Time.deltaTime*1.5f);
            pulseText.text = min + ":" + ((sec < 10) ? "0" + sec : "" + sec);
            text.color = new Color(1, 1 - pulse / 2, 1 - pulse / 2);
            pulseText.color = new Color(1, 1 - pulse / 2, 1 - pulse / 2, pulse);
            pulseText.transform.localScale = new Vector3(3 - pulse*2, 3 - pulse*2, 1);
        }
        else
        {
            pulseText.color = new Color(1,1,1,0);
        }
    }

    void CheckForThump(int sec)
    {
        if (sec==oldSec)
        {
            return;
        }
        if (sec == 30 || sec == 20 || sec <= 10)
        {
            SoundManager.Instance.PlaySound("StealthOn", 1f);
            pulse = 1;
        }
    }
}
