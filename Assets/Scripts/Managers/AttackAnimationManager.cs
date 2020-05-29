using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : Singleton<AttackAnimationManager>
{

    public Enemy monsterStats;
    public CharacterStats playerStats;

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
    public float hopSwingSoundPoint = .2f;

    public float defaultAttackFrameTime = 0.075f;
    public float timeWarp = 1;

    public float initialBlastImpactTime = .35f;
    public float blastRecoilFinishTime = .55f;
    public Vector2 blastRecoilAmount = new Vector2(.2f, .2f);
    public float blastRecoilFriction = .5f;
    public float returnRecoilStart = .75f;
    public float returnRecoilReturnDuration = .25f;
    public float blastWarmupDuration = .2f;
    public Vector4 blastFrameTime= new Vector4(.25f,.075f,.075f,.2f);
    public float blastSoundPoint = .2f;
    internal float orbitalLaserSoundPoint = .05f;
    public float initialThrustImpactTime = .4f;
    public float stationaryThrustWarmupDuration = .06f;
    public Vector4 thrustFrameTime = new Vector4(.06f, .06f, .06f, .3f);
    public float thrustSoundPoint = .2f;
    public float[] orbitalLaserFrameTimes = new float[] { .1f, .167f, .25f, .3f, .35f, .433f, .5f};
    public float dumpStartRunUpTime = .01f;
    public float dumpEndRunUpTime = .2f;
    public float returnFromDumpStart = .6f;
    public float returnFromDumpEnd = 1.2f;
    public float dumpSoundPoint = .2f;
    public Vector4 dumpFrameTime = new Vector4(.3f, .367f, .45f, .533f);
    public float dumpEndFrameTime=.7f;
    public float thrustStartTime = .01f;
    public float thrustRunTime = .15f;
    public float thrustRecoilEndTime = .5f;
    public float returnFromThrustStart = .6f;
    public float returnFromThrustEnd = 1.2f;
    public float thrustWindupDistance = .2f;
    public float thrustOvershootDistance = .1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCombatPawns(Enemy monsterStats, CharacterStats playerStats)
    {
        this.monsterStats = monsterStats;
        this.playerStats = playerStats;
    }
}
