using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMonsterSpriter : MonoBehaviour
{
    private Renderer r;

    public GameObject hero;
    public Renderer mapr;

    int subframe = 0;
    public int frame;
    //public int frameTime;
    // Start is called before the first frame update
    void Start()
    {
        this.r = this.GetComponent<Renderer>();       //Speeds things up to only get the renderer once.
        r.material = new Material(r.material); //Copies the material. If you don't do this, all monsters with this material will share all properties you set (notably, the frame.)
    }

    // Update is called once per frame
    void Update()
    {
        if (subframe++ > 20)
        {
            r.material.SetInt("_Frame", frame++);
            subframe = 0;
        }

        //Make sure the monster is lit according to where the hero is:
        r.material.SetVector("_HeroXY", hero.transform.position/10);

        //this shouldn't be tied to the monster, but I wanted to keep all "sample code" in a single script. Anyway, this also sets the map's renderer to recognize where the hero's light is.
        mapr.material.SetVector("_HeroXY", hero.transform.position/10);
    }
}
