using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyNaniCamera : MonoBehaviour
{
    private bool init;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!init&&Camera.main!=null)
        {
            this.transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
        }
        if (GameData.Instance.isCutscene) {
            this.transform.SetPositionAndRotation(Camera.main.transform.localPosition, Camera.main.transform.localRotation);
        }
    }
}
