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
    }

    // Update is called once per frame
    void Update()
    {
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
            this.transform.localScale = new Vector3(Mathf.Sin(Mathf.PI * percentThrough), Mathf.Sin(Mathf.PI * percentThrough), Mathf.Sin(Mathf.PI * percentThrough));
            this.transform.position = new Vector3(
                    startPosition.x + (endPosition.position.x - startPosition.x) * percentThrough,
                    startPosition.y + (endPosition.position.y - startPosition.y) * percentThrough + 4 * height * (-1 * (percentThrough - .5f) * (percentThrough - .5f) + .25f),
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
                color = Color.cyan;
                break;
            case CrystalType.HEALTH:
                color = Color.yellow;
                break;
            case CrystalType.MANA:
                color = Color.magenta;
                break;
        }

        main.startColor = color;
    }

    public void OnImpact()
    {
        collectionSound.Play();
    }
}
