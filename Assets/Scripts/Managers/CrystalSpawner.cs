using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrystalSpawner 
{


    public static void SpawnCrystalParticles(CrystalType type, int crystalsGained, CharacterStats playerData, GameObject spawningObject, PowerupEffect powerUpEffect)
    {
        //SoundManager.Instance.PlaySound("CrystalShatter", 1);
        int numberOfParticles = Mathf.RoundToInt(Mathf.Sqrt(crystalsGained));

        List<float> delays = new List<float>();
        for (int x = 0; x < numberOfParticles; x++)
        {
            float delay = UnityEngine.Random.Range(0, Mathf.Sqrt(x) / 35);
            delays.Add(delay);
        }

        delays.Sort();
        //PowerupEffect powerUpEffect = PowerupEffect();

        foreach (float delay in delays)
        {
            PowerupEffect pe = GameObject.Instantiate<PowerupEffect>(powerUpEffect, spawningObject.transform.position, Quaternion.identity);
            pe.Initialize(spawningObject.transform.GetChild(0).position, playerData.transform.GetChild(0).GetChild(0), delay, type);
        }
    }
}
