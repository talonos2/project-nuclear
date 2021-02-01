using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePowerInBossRoomEffect : MonoBehaviour
{
    public ParticleSystem particles;
    public float duration;
    private Action onComplete;
    public float height;

    private float delay = .05f;
    private Transform startPosition;
    private Transform endPosition;
    private float timeAlive;
    private bool hasStarted = false;
    private bool hasImpacted = false;
    private CrystalType type;
    internal bool hideCrystalSounds;

    public AudioSource collectionSound;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.isInBattle || GameState.getFullPauseStatus())
        {
            var main = particles.main;
            main.simulationSpeed = .0000001f;
            return;
        }
        else
        {
            var main = particles.main;
            main.simulationSpeed = 1;
        }
        timeAlive += Time.deltaTime;

        if (!hasStarted && timeAlive > delay)
        {
            hasStarted = true;
            particles.gameObject.SetActive(true);
        }

        if (hasStarted)
        {
            float percentThrough = ((timeAlive - delay) / duration);

            float distance = percentThrough * percentThrough;

            //Warning, math ahead.
            this.transform.localScale = new Vector3(Mathf.Sin(Mathf.PI * Mathf.Pow(1 - percentThrough, 2)), Mathf.Sin(Mathf.PI * Mathf.Pow(1 - percentThrough, 4)), .0000000001f);
            this.transform.position = new Vector3(
                    startPosition.position.x + (endPosition.position.x - startPosition.position.x) * distance,
                    startPosition.position.y + (endPosition.position.y - startPosition.position.y) * distance,
                    startPosition.position.z + (endPosition.position.z - startPosition.position.z) * distance);
            if (!hasImpacted && percentThrough >= 1)
            {
                OnImpact();
                hasImpacted = true;
            }
            if (percentThrough >= 2)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    public void Initialize(Transform startPosition, Transform endPosition, float duration, Action onComplete, ElementalPower type)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;

        var main = particles.main;  //I don't know why this step is necessary, but it is. :/
        main.startLifetime = duration;
        main.startColor = type.GetColor();
        this.duration = duration;
        this.onComplete = onComplete;
    }

    public void OnImpact()
    {
        onComplete?.Invoke();
    }
}
