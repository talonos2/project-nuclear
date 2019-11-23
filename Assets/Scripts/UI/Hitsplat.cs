using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hitsplat : MonoBehaviour
{
    float baseHeight = 2;
    float baseYVelo = .2f;
    Vector3 startPosition;
    float gravity = .03f;
    float bounciness = .5f;
    int maxBounces = 4;
    int numberOfFramesAppearing = 25;

    float height;
    float yVelo;
    int bounces;

    bool isBouncing = true;
    int framesAppeared = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().sortingOrder = 11;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBouncing)
        {
            transform.position = new Vector3(0, height, 0)+startPosition;
            height += yVelo;
            yVelo -= gravity;
            if (height <= baseHeight)
            {
                yVelo *= -bounciness;
                bounces -= 1;
                if (bounces <= 0)
                {
                    isBouncing = false;
                }
            }
        }
        if (framesAppeared++ >= numberOfFramesAppearing)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(int damage, Color color)
    {
        TextMeshPro text = this.GetComponent<TextMeshPro>();
        text.text = ""+damage;
        text.color = color;
        yVelo = baseYVelo;

        height = baseHeight;
        bounces = maxBounces;

        this.startPosition = this.transform.position;

        Update();
    }
}
