using Naninovel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public GameObject cutScenePlayer;
    public string[] cutScenes;
    public Vector2[] cameraLocation;
    public Vector2[] introCameraLocation;
    internal static bool introCutscene;
    internal static bool endCutscene;
    private static int introSceneNumber;
    internal static bool postRun1Cutscene;
    internal static bool runTownBackDialogue;
    internal static bool waitingForScriptPlyr;
    internal static string nextDialogue;
    internal static bool dialogueWaiting;

    void Update()
    {
        if (waitingForScriptPlyr) { return; }
        if (dialogueWaiting)
        {
            dialogueWaiting = false;
            waitingForScriptPlyr = true;
            GameData.Instance.isInDialogue = true;
            Naninovel.Engine.Reset();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RuntimeInitializer.InitializeAsync();
            Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(nextDialogue);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }


    }

    internal static void LoadCutsceneAndWorldSpaceFade(float fadeDuration)
    {
        GameData.Instance.isCutscene = true;
        String sceneToTransitionTo = GetMapInWhichNextCutsceneTakesPlace();
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.InitNext(sceneToTransitionTo, fadeDuration);
    }

    public static void SetNextDialogue(string aDialogue)
    {
        nextDialogue = aDialogue;
        dialogueWaiting = true;
    }

    public static void LoadEnding()
    {
        GameData.Instance.isCutscene = true;
        SceneManager.LoadScene("WinScreen");
        //Console.Write("ending ends");
        return;
    }


    public static void LoadCutscene()
    {
        GameData.Instance.isCutscene = true;
        String sceneToTransitionTo = GetMapInWhichNextCutsceneTakesPlace();
        SceneManager.LoadScene(GetMapInWhichNextCutsceneTakesPlace());
    }

    public static void LoadCutsceneAndFade(Canvas c, float fadeDuration)
    {
        GameData.Instance.isCutscene = true;
        String sceneToTransitionTo = GetMapInWhichNextCutsceneTakesPlace();
        FadeOut fadeout = GameObject.Instantiate<FadeOut>(Resources.Load<FadeOut>("Fade Out Plane"));
        fadeout.attachToGUI(c);
        fadeout.InitNext(sceneToTransitionTo, fadeDuration);
    }

    public static String GetMapInWhichNextCutsceneTakesPlace()
    {
        if (introCutscene)
        {
            switch (introSceneNumber)
            {
                case 0:
                    return "TownMap_1";
                case 1:
                    return "TownInterior_Manor_1";
                case 2:
                    return "TownMap_1";
            }
            return null;
        }

        if (postRun1Cutscene)
        {
            return "TownMap_1";
        }

        switch (GameData.Instance.RunNumber)
        {
            case 1:
                return "TownMap_1";
            case 2:
                SceneManager.LoadScene("TownInterior_Pub_1");
                break;
            case 3:
                return "TownMap_1";
            case 4:
                return "TownMap_1";
            case 5:
                return "TownMap_1";
            case 6:
                return "TownMap_1"; //Blacksmith
            case 7:
                return "TownInterior_Church_1";
            case 8:
                return "TownInterior_Pub_1";
            case 9:
                return "TownMap_1";
            case 10:
                return "TownMap_1";
            case 11:
                return "TownMap_1";
            case 12:
                return "TownMap_1";
            case 13:
                return "TownInterior_Pub_1";
            case 14:
                return "TownInterior_Manor_1";
            case 15:
                return "TownMap_1";
            case 16:
                return "TownInterior_SeersCottege_1"; //sagehut
            case 17:
                return "TownMap_1";
            case 18:
                return "TownMap_1";
            case 19:
                return "TownMap_1";
            case 20:
                return "TownMap_1";
            case 21:
                return "TownInterior_Manor_1";
            case 22:
                return "TownMap_1";
            case 23:
                return "TownMap_1";
            case 24:
                return "TownMap_1";
            case 25:
                return "TownInterior_Manor_1";
            case 26:
                return "TownInterior_Manor_1";
            case 27:
                return "TownMap_1";
            case 28:
                return "TownInterior_SeersCottege_1";
            case 29:
                return "TownMap_1";
            case 30:
                return "TownMap_1";
        }
        return null;
    }



    public void RunCutscene()
    {

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        GameData gameData = GameData.Instance;

        if (postRun1Cutscene)
        {
            if (GameData.Instance.hiroDeathMonster)
            {
                Instantiate(cutScenePlayer, new Vector3(cameraLocation[34].x, cameraLocation[34].y, 0), Quaternion.identity);
                InitAndRunCutscene(cutScenes[34]);
            }
            else
            {
                Instantiate(cutScenePlayer, new Vector3(cameraLocation[35].x, cameraLocation[35].y, 0), Quaternion.identity);
                InitAndRunCutscene(cutScenes[35]);
            }
            return;
        }

        if (introCutscene)
        {
            Instantiate(cutScenePlayer, new Vector3(cameraLocation[31 + introSceneNumber].x, cameraLocation[31 + introSceneNumber].y, 0), Quaternion.identity);
            InitAndRunCutscene(cutScenes[31 + introSceneNumber]);
            introSceneNumber += 1;
            if (introSceneNumber > 2)
                introCutscene = false;
        }
        else
        {
            Instantiate(cutScenePlayer, new Vector3(cameraLocation[gameData.RunNumber].x, cameraLocation[gameData.RunNumber].y, 0), Quaternion.identity);
            InitAndRunCutscene(cutScenes[gameData.RunNumber]);
        }

        // RuntimeInitializer.InitializeAsync();
        // Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(cutScenes[gameData.RunNumber]);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

    }

    public async void InitAndRunCutscene(string cutSceneName)
    {
        //await Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(cutScenes[GameData.Instance.RunNumber]);
        //await Engine.GetService<StateManager>().ResetStateAsync();
        Naninovel.Engine.Reset();
        await RuntimeInitializer.InitializeAsync();
        await Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync(cutSceneName);
    }

}


/*
public static void RunStartOfTownDialog()
{
    Debug.Log("start of town dialog");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

    GameData.Instance.isInDialogue = true;
    Naninovel.Engine.Reset();
    RuntimeInitializer.InitializeAsync();
    Engine.GetService<ScriptPlayer>().PreloadAndPlayAsync("enterTown");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}*/
