using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpper : MonoBehaviour
{
    private ParticleSystem particles;
    private int unsungLevels = 0;
    private float timeSinceLastLevel = 0;
    private static readonly float TIME_BETWEEN_LEVELS = 60f/160f;
    // Start is called before the first frame update
    void Start()
    {
        this.particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastLevel += Time.deltaTime;
        if (timeSinceLastLevel > TIME_BETWEEN_LEVELS && unsungLevels > 0)
        {
            unsungLevels--;
            timeSinceLastLevel = 0;
            SoundManager.Instance.PlaySound("LevelUp",1);
            particles.Emit(1);
        }
    }

    internal void AddLevel()
    {
        unsungLevels++;
    }

    internal void ShutUp()
    {
        unsungLevels = 1;
    }
}
