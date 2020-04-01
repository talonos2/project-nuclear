using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxHandler : MonoBehaviour
{
    public float parallaxAmount;
    public Vector2 parallaxOffset;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
            if (!mainCamera)
            {
                return;
            }
        }
        Vector3 newPosition = mainCamera.transform.position / parallaxAmount;
        newPosition.z = this.transform.position.z;
        newPosition += new Vector3(parallaxOffset.x, parallaxOffset.y, 0);
        this.transform.position = newPosition;
    }
}
