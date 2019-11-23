using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchbladeScript : MonoBehaviour
{
    public Sprite[] switchbladeSprites;
    private bool open = false;
    private int spriteNum = 0;
    private bool close = false;

    public Transform swayBall;
    public GameObject errorParticlePrefab;
    public GameObject goodParticlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            spriteNum++;
            this.GetComponent<SpriteRenderer>().sprite = switchbladeSprites[spriteNum];
            if (spriteNum == 9)
            {
                open = false;
            }
        }
        if (close)
        {
            spriteNum--;
            this.GetComponent<SpriteRenderer>().sprite = switchbladeSprites[spriteNum];
            if (spriteNum == 0)
            {
                close = false;
            }
        }
    }

    public void StartOpen()
    {
        open = true;
        close = false;
        spriteNum = 0;
    }

    public void StartClose()
    {
        spriteNum = 6;
        open = false;
        close = true;
    }

    internal void HandleRightSideSway(float time)
    {
        float x = (1-Mathf.Pow(1f-Mathf.Sin(time * Mathf.PI),1.4f)) * 7.25f;
        float y = .5f - Mathf.Abs(time - .5f);
        swayBall.localPosition = new Vector3(x, 4.8f+y, 0);
    }

    internal void HandleLeftSideSway(float time)
    {
        float x = (1-Mathf.Pow(1-Mathf.Sin(time * Mathf.PI),1.4f)) * 7.25f;
        float y = .5f - Mathf.Abs(time - .5f);
        swayBall.localPosition = new Vector3(-x, 4.8f + y, 0);
    }

    internal void SpawnErrorParticles()
    {
        GameObject particles = GameObject.Instantiate(errorParticlePrefab, new Vector3(swayBall.position.x, swayBall.position.y, swayBall.position.z), Quaternion.Euler(0,0,0));
    }

    internal void SpawnGoodParticles()
    {
        GameObject particles = GameObject.Instantiate(goodParticlePrefab, new Vector3(transform.position.x, transform.position.y+4.8f, transform.position.z), Quaternion.Euler(0, 0, 0));
    }
}
