using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUIOn : MonoBehaviour
{
    public GameObject[] stuffToTurnOff;

    public void turnOffUi() {
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(false);
        }
    }

    public void turnOnUi() {
        foreach (GameObject t in stuffToTurnOff)
        {
            t.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.isInDialogue) {
            turnOffUi();
        }

        if (!GameData.Instance.IsInTown() && !GameData.Instance.isInDialogue) {
            turnOnUi();
        }

        if (GameData.Instance.IsInTown()) {
            foreach (GameObject t in stuffToTurnOff)
            {
                t.SetActive(!GameData.Instance.IsInTown());
            }
        }

    }
}
