using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleParticleEmitter : MonoBehaviour
{
    private ParticleSystem particles;
    private int queuedParticles = 0;
    public float timeBetweenParticles = 60f / 160f;

    private float timeSinceLastParticle = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastParticle += Time.deltaTime;
        if (timeSinceLastParticle > timeBetweenParticles && queuedParticles > 0)
        {
            queuedParticles--;
            timeSinceLastParticle = 0;
            particles.Emit(1);
        }
    }

    public void EmitSingleParticle()
    {
        queuedParticles++;
    }

    public void EmitSeveralParticles(int number)
    {
        queuedParticles+= number;
    }

    public void TrimQueue()
    {
        queuedParticles = 1;
    }

    public void CancelQueue()
    {
        queuedParticles = 0;
    }
}
