using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSound : MonoBehaviour
{
    public string toPlay;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound(toPlay, 1);
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
