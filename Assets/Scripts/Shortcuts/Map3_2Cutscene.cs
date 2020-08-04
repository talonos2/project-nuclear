using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Map3_2Cutscene : MonoBehaviour
{

    public GameObject Door1;
    public GameObject Door2;
    public float delayTillOpen = 2;
    public float delayUntilLeave = 1.5f;
    private bool doorsOpen = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        delayTillOpen -= Time.deltaTime;
        if (delayTillOpen <= 0 && !doorsOpen) {
            GameData.Instance.map3_2Shortcut = true;
            doorsOpen = true;
            Door1.GetComponent<DoorShortcutController>().OpenDoors();
            Door2.GetComponent<DoorShortcutController>().OpenDoors();
            //Play open door sounds
        }
        if (doorsOpen) {
            delayUntilLeave -= Time.deltaTime;
            if (delayUntilLeave <= 0) {
                //Destroy scene, and un-battle pause game
            }
        }
    }
}
