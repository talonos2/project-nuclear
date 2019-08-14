using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurredBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        // do something with texture

        // cleanup
        Object.Destroy(texture);
    }

    public void LateUpdate()
    {
        StartCoroutine(RecordFrame());
    }

}
