using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1_3Shortcut : MonoBehaviour
{
    public Map1_3Shortcut monster1;
    public Map1_3Shortcut monster2;
    public Map1_3Shortcut monster3;
    public Map1_3Shortcut monster4;

    // Start is called before the first frame update
    void Start()
    {
        //if (GameData.Instance.earthBoss2 && !GameData.Instance.map1_3Shortcut)
        //{
        //    GameData.Instance.map1_3Shortcut = true;

        //}

        //if (GameData.Instance.map)
        //{
         ///   Destroy(this.gameObject);
        //}


    }

    public void setupShortcutAlert() {
        monster1.gameObject.GetComponent<MonsterMovement>().hazardIcon.enabled = true;
        monster1.GetComponent<MonsterMovement>().SetNewMovespeed(monster1.GetComponent<MonsterMovement>().MoveSpeed * 2);
        monster2.gameObject.GetComponent<MonsterMovement>().hazardIcon.enabled = true;
        monster2.GetComponent<MonsterMovement>().SetNewMovespeed(monster2.GetComponent<MonsterMovement>().MoveSpeed * 2);
        monster3.gameObject.GetComponent<MonsterMovement>().hazardIcon.enabled = true;
        monster3.GetComponent<MonsterMovement>().SetNewMovespeed(monster3.GetComponent<MonsterMovement>().MoveSpeed * 2);
        monster4.gameObject.GetComponent<MonsterMovement>().hazardIcon.enabled = true;
        monster4.GetComponent<MonsterMovement>().SetNewMovespeed(monster4.GetComponent<MonsterMovement>().MoveSpeed * 2);
    }
    public void setupShortcut() {
        Destroy(monster1.gameObject);
        Destroy(monster2.gameObject);
        Destroy(monster3.gameObject);
        Destroy(monster4.gameObject);
    }

   
}
