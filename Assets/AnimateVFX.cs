using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateVFX : MonoBehaviour
{
    public Sprite[] sprites;
    private float timeSoFar = 0;
    private SpriteRenderer image;
    public float timePerFrame = .06f;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<SpriteRenderer>();
        Debug.Log("I am here! " + name);
    }

    // Update is called once per frame
    void Update()
    {
        timeSoFar += Time.deltaTime;
        if (timeSoFar > sprites.Length*timePerFrame)
        {
            Destroy(this.gameObject);
        }
        else
        {
            image.sprite = sprites[(int)(timeSoFar / timePerFrame)];
        }
    }
}
