using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : Singleton<AttackAnimationManager>
{
    public float initialHopDuration = .35f;
    public float enemyKnockBackStart = .35f;
    public float enemyKnockBackDuration=.1f;
    public float enemySpringbackStart=.45f;
    public float enemySpringbackDuration=.05f;
    public float enemyMoveBackStart=.7f;
    public float enemyMoveBackDuration=.2f;
    public float returnHopStart=.6f;
    public float returnHopDuration=.6f;

    public float initialHopHeight = .5f;
    public float returnHopHeight = .05f;
    public float knockBackXOffset = .15f;
    public float springBackXOffset = .1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
