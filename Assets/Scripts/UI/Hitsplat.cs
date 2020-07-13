using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hitsplat : MonoBehaviour
{
    public float baseHeight = 2;
    public float baseYVelo = .2f;
    Vector3 startPosition;
    public float gravity = .03f;
    public float bounciness = .5f;
    public int maxBounces = 4;
    public int numberOfFramesAppearing = 25;

    private float height;
    private float yVelo;
    private int bounces;

    private bool isBouncing = true;
    private int framesAppeared = 0;

    protected int physicalDamage = 0;
    protected int elementalDamage = 0;
    protected bool crit = false;
    protected bool elementalCrit = false;
    protected bool goodTiming = false;
    protected bool effective = false;
    protected ElementalPower type;

    public TextMeshPro text1;
    public TextMeshPro text2;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isBouncing)
        {
            transform.position = new Vector3(startPosition.x, height + startPosition.y, -99);
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

    public virtual void Init(int physicalDamage, int elementalDamage, bool goodTiming, bool effective, bool crit, bool elementalCrit, ElementalPower elementalType)
    {
        yVelo = baseYVelo;
        height = baseHeight;
        bounces = maxBounces;
        this.startPosition = this.transform.position;

        this.physicalDamage = physicalDamage;
        this.elementalCrit = elementalCrit;
        this.elementalDamage = elementalDamage;
        this.effective = effective;
        this.crit = crit;
        this.type = elementalType;
        this.goodTiming = goodTiming;

        CreateGraphics();
        Update();
    }

    protected virtual void CreateGraphics()
    {
        text1.SetText("This is an interface,\nnot a real hitsplat!");
    }
}
