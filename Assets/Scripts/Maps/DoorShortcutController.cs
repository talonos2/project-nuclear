using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorShortcutController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject openedDoor;
    void Start()
    {
        if (GameData.Instance.Map3_2Shortcut) {
            Instantiate(openedDoor, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void OpenDoors() {
        if (GameData.Instance.Map3_2Shortcut)
        {
            Instantiate(openedDoor, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
