﻿using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneLoader : MonoBehaviour
{
    public GameObject cutScenePlayer;
    public string[] cutScenes;
    public Vector2[] cameraLocation;

    public void LoadCutsceneLevel() {

        switch (GameData.Instance.RunNumber)
        {
            case 1:
                Instantiate(cutScenePlayer, new Vector3(cameraLocation[GameData.Instance.RunNumber - 1].x, cameraLocation[GameData.Instance.RunNumber - 1].y, 0), Quaternion.identity);
                Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(cutScenes[GameData.Instance.RunNumber - 1]);
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;
            case 12:

                break;
            case 13:

                break;
            case 14:

                break;
            case 15:

                break;
            case 16:

                break;
            case 17:

                break;
            case 18:

                break;
            case 19:

                break;
            case 20:

                break;
            case 21:

                break;
            case 22:

                break;
            case 23:

                break;
            case 24:

                break;
            case 25:

                break;
            case 26:

                break;
            case 27:

                break;
            case 28:

                break;
            case 29:

                break;
            case 30:

                break;
        }
    } 
    public void RunCutscene() {
        RuntimeInitializer.InitializeAsync();
        GameData gameData = GameData.Instance;
        switch (gameData.RunNumber) {
            case 1:
                Instantiate(cutScenePlayer, new Vector3(cameraLocation[gameData.RunNumber-1].x, cameraLocation[gameData.RunNumber - 1].y, 0), Quaternion.identity);
                Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(cutScenes[gameData.RunNumber-1]);
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;
            case 12:

                break;
            case 13:

                break;
            case 14:

                break;
            case 15:

                break;
            case 16:

                break;
            case 17:

                break;
            case 18:

                break;
            case 19:

                break;
            case 20:

                break;
            case 21:

                break;
            case 22:

                break;
            case 23:

                break;
            case 24:

                break;
            case 25:

                break;
            case 26:

                break;
            case 27:

                break;
            case 28:

                break;
            case 29:

                break;
            case 30:

                break;
        }
    }
}
