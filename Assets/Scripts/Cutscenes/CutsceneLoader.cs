using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public GameObject cutScenePlayer;
    public string[] cutScenes;
    public Vector2[] cameraLocation;

    public static void LoadCutscene()
    {
        GameData.Instance.isCutscene = true;
        switch (GameData.Instance.RunNumber)
        {
            case 1:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 2:
                SceneManager.LoadScene("TownInterior_Pub_1");
                break;
            case 3:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 4:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 5:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 6:
                SceneManager.LoadScene("TownMap_1");
                //blacksmith
                break;
            case 7:
                SceneManager.LoadScene("TownInterior_Church_1");
                break;
            case 8:
                SceneManager.LoadScene("TownInterior_Pub_1");
                break;
            case 9:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 10:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 11:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 12:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 13:
                SceneManager.LoadScene("TownInterior_Pub_1");
                break;
            case 14:
                SceneManager.LoadScene("TownInterior_Manor_1");
                break;
            case 15:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 16:
                SceneManager.LoadScene("TownInterior_SeersCottege_1");
                //sagehut
                break;
            case 17:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 18:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 19:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 20:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 21:
                SceneManager.LoadScene("TownInterior_Manor_1");
                break;
            case 22:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 23:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 24:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 25:
                SceneManager.LoadScene("TownInterior_Manor_1");
                break;
            case 26:
                SceneManager.LoadScene("TownInterior_Manor_1");
                break;
            case 27:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 28:
                SceneManager.LoadScene("TownInterior_SeersCottege_1");
                break;
            case 29:
                SceneManager.LoadScene("TownMap_1");
                break;
            case 30:
                SceneManager.LoadScene("TownMap_1");
                break;
        }
    } 
    public void RunCutscene() {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        GameData gameData = GameData.Instance;
        Instantiate(cutScenePlayer, new Vector3(cameraLocation[gameData.RunNumber].x, cameraLocation[gameData.RunNumber].y, 0), Quaternion.identity);

        RuntimeInitializer.InitializeAsync();
        Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(cutScenes[gameData.RunNumber]);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

    }
}
