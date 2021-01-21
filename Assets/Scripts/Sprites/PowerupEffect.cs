using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEffect : MonoBehaviour
{
    public ParticleSystem particles;
    public float duration;
    public float height;

    private float delay;
    private Vector3 startPosition;
    private Transform endPosition;
    private float timeAlive;
    private bool hasStarted = false;
    private bool hasImpacted = false;
    private CrystalType type;

    public AudioSource collectionSound;

    // Use this for initialization
    void Start()
    {
        var main = particles.main;  //I don't know why this step is necessary, but it is. :/
        main.startLifetime = duration;
        collectionSound.clip = Resources.Load<AudioClip>("Sounds/GetEnergy");
        //collectionSound.pitch = Random.Range(.9f,1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.isInBattle || GameState.fullPause)
        {
            var main = particles.main;  //I don't know why this step is necessary, but it is. :/
            main.simulationSpeed = .0000001f;
            return;
        }
        else
        {
            var main = particles.main;  //I don't know why this step is necessary, but it is. :/
            main.simulationSpeed = 1;
        }
        timeAlive += Time.deltaTime;
        if (!hasStarted && timeAlive > delay)
        {
            hasStarted = true;
            particles.gameObject.SetActive(true);
            //particles.Emit(1);
        }

        if (hasStarted)
        {
            float percentThrough = (timeAlive - delay) / duration;

            //Warning, math ahead.
            this.transform.localScale = new Vector3(Mathf.Sin(Mathf.PI * percentThrough), Mathf.Sin(Mathf.PI * percentThrough), .0000000001f);
            this.transform.position = new Vector3(
                    startPosition.x + (endPosition.position.x - startPosition.x) * percentThrough,
                    startPosition.y + (endPosition.position.y - startPosition.y) * percentThrough,
                    startPosition.z + (endPosition.position.z - startPosition.z) * percentThrough);
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

    public void Initialize(Vector3 startPosition, Transform endPosition, float delay, CrystalType type)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.delay = delay;
        this.type = type;

        Color color = Color.white;

        var main = particles.main;  //I don't know why this step is necessary, but it is. :/

        switch (type)
        {
            case CrystalType.ATTACK:
                color = new Color(1, .3f, .3f);
                break;
            case CrystalType.DEFENSE:
                color = Color.yellow;
                break;
            case CrystalType.HEALTH:
                color = Color.green;
                break;
            case CrystalType.MANA:
                color = Color.cyan;
                break;
        }

        main.startColor = color;
    }

    public void OnImpact()
    {
        SoundManager.Instance.PlaySound("GetEnergy", 1f);
    }
}
