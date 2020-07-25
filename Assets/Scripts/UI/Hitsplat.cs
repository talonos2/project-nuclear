using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hitsplat : MonoBehaviour
{
    public float baseHeight = 2;
    public float baseYVelo = .2f;
    public float gravity = .03f;
    public float bounciness = .5f;
    public int maxBounces = 4;
    public int numberOfFramesAppearing = 25;

    private float[] height;
    private float[] yVelo;
    private Vector3[] startPosition;
    public Transform[] thingsToBounce;
    private int[] bounces;
    private float[] appearDelays;

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
    public float timeBetweenAppearances = .1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < thingsToBounce.Length; x++)
        {
            if (isBouncing)
            {
                if (appearDelays[x] > 0)
                {
                    appearDelays[x] -= Time.deltaTime;
                    continue;
                }
                thingsToBounce[x].gameObject.SetActive(true);

                thingsToBounce[x].position = new Vector3(startPosition[x].x, height[x] + startPosition[x].y, -99);
                height[x] += yVelo[x];
                yVelo[x] -= gravity;
                if (height[x] <= baseHeight)
                {
                    height[x] = baseHeight+float.Epsilon;
                    yVelo[x] *= (float)(-bounciness);
                    bounces[x] -= 1;
                    if (bounces[x] <= 0)
                    {
                        isBouncing = false;
                    }
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
        yVelo = new float[thingsToBounce.Length];
        height = new float[thingsToBounce.Length];
        bounces = new int[thingsToBounce.Length];
        this.startPosition = new Vector3[thingsToBounce.Length];
        appearDelays = new float[thingsToBounce.Length];
        for (int x = 0; x < thingsToBounce.Length; x++)
        {
            yVelo[x] = baseYVelo;
            height[x] = baseHeight;
            bounces[x] = maxBounces;
            this.startPosition[x] = thingsToBounce[x].position;
            appearDelays[x] = x * timeBetweenAppearances;
        }

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
