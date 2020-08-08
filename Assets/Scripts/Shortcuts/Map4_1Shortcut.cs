using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map4_1Shortcut : MonoBehaviour
{
    public List<GameObject> monstersToDestroy = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.Instance.map4_1Shortcut) {
            foreach (GameObject monster in monstersToDestroy) {
                Destroy(monster);
            }

        }
    }

}
