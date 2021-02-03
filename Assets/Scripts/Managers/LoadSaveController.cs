using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Windows;

public class LoadSaveController : MonoBehaviour
{

    public SaveSlotUiController saveSlotPrefab1;
    public SaveSlotUiController saveSlotPrefab2;
    public SaveSlotUiController saveSlotPrefab3;
    public SaveSlotUiController saveSlotPrefab4;
    public SaveSlotUiController saveSlotPrefab5;
    public Image saveSelector;
    public TextMeshProUGUI saveOrLoadText;
    //public GameObject itemContainer;
    //public bool loadGame;
    //public bool saveGame;
    internal int saveSlotSelected = 0;

    public Canvas aCanvasThis;
    public static bool isListLoaded;
    public static List<GameSaverManager> savedDataList = new List<GameSaverManager>();
    //public NewGameController newGameButton;
    public bool loadSaveActive;
    private EscapeKeyController callingEscapeKeyControllerScriptReturn;
    private bool wait1Frame;
    private static int pointLocation = 0;
    private PauseMenuController callingPauseMenuControllerScriptReturn;
    private NewGameController callingGameControllerScriptReturn;
    private TownEscapeKeyController callingTownEscapeScriptReturn;
    private float selectionDelayDefault = .15f;
    private float selectionDelay = 0;
    private float speedupTimeDefault = 1.25f;
    private float speedupTimer;
    private bool townSave;
    private bool townLoad;
    private bool dungeonLoad;
    private bool pauseLoad;
    private bool loadingGame = false;
    public NewGameController newGameController;

    // Start is called before the first frame update

    void Start()
    {
        if (!isListLoaded)
        {
            LoadSavedDataToList("Autosave");
            for (int i = 1; i < 31; i++)
            {
                LoadSavedDataToList("SaveSlot" + i);
            }
            isListLoaded = true;
        }


        //SetupAllSaveUI(saveSlotSelected);
        aCanvasThis.enabled = false;



    }

    private void Update()
    {
        if (!loadSaveActive) return;
        if (!wait1Frame) { wait1Frame = true; return; }

        if (FWInputManager.Instance.GetKeyDown(InputAction.ACTIVATE))
        {



            SoundManager.Instance.PlaySound("MenuOkay", 1f);
            if (townSave)
            {
               // SaveSlotUiController tempUiSaveController = getUiSaveSlot();
                //tempUiSaveController.savingOverlay.SetActive(true);
                SaveGame(saveSlotSelected);
                SetupAllSaveUI(saveSlotSelected);

            }
            else
            {
                GameSaverManager gameSaver = new GameSaverManager();
                gameSaver = savedDataList[saveSlotSelected];
                if (gameSaver.RunNumber == 0)
                {
                    SoundManager.Instance.PlaySound("MenuNope", 1f);
                    return;
                }
                SetupLoadingGame();

            }

        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_UP))
        {
            selectionDelay = selectionDelayDefault + .4f;
            speedupTimer = speedupTimeDefault;
            saveSlotSelected -= 1;
            if (saveSlotSelected < 0)
                saveSlotSelected = 30;
            SoundManager.Instance.PlaySound("MenuMove", 1f);
            pointLocation--;
            if (pointLocation < 0)
                pointLocation = 0;
            SetupAllSaveUI(saveSlotSelected);
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.MENU_DOWN))
        {
            selectionDelay = selectionDelayDefault + .4f;
            speedupTimer = speedupTimeDefault;
            saveSlotSelected += 1;
            if (saveSlotSelected > 30)
                saveSlotSelected = 0;
            SoundManager.Instance.PlaySound("MenuMove", 1f);
            pointLocation++;
            if (pointLocation > 4)
                pointLocation = 4;
            SetupAllSaveUI(saveSlotSelected);
        }

        if (FWInputManager.Instance.GetKeyDown(InputAction.GO_BACK))
        {
            DeactivateLoad();

            //if ((townSave || townLoad)&& ! pauseLoad) callingTownEscapeScriptReturn.ReActivate();
            if (dungeonLoad) callingEscapeKeyControllerScriptReturn.ReActivate();
            else if (pauseLoad) callingPauseMenuControllerScriptReturn.ReActivate();
            else callingGameControllerScriptReturn.ReActivate();
            pauseLoad = false;
            townSave = false;
            townLoad = false;
            dungeonLoad = false;
        }


    }



    private void SetupAllSaveUI(int saveSlot)
    {
        saveSelector.GetComponent<RectTransform>().localPosition = new Vector3(-206, 125 - 80 * pointLocation, 0);

        //if (saveSlot < 5) { saveSlot = 0; }
        saveSlot -= pointLocation;
        if (saveSlot < 0)
            saveSlot = 31 + saveSlot;

        GameSaverManager savedGame = savedDataList[saveSlot];
        saveSlotPrefab1.SetupSaveSlotUI(savedGame.iceBoss1, savedGame.earthBoss1, savedGame.fireBoss1, savedGame.airBoss1, savedGame.RunNumber, saveSlot);
        saveSlot += 1;
        if (saveSlot > 30) { saveSlot = 0; }

        savedGame = savedDataList[saveSlot];
        saveSlotPrefab2.SetupSaveSlotUI(savedGame.iceBoss1, savedGame.earthBoss1, savedGame.fireBoss1, savedGame.airBoss1, savedGame.RunNumber, saveSlot);
        saveSlot += 1;
        if (saveSlot > 30) { saveSlot = 0; }

        savedGame = savedDataList[saveSlot];
        saveSlotPrefab3.SetupSaveSlotUI(savedGame.iceBoss1, savedGame.earthBoss1, savedGame.fireBoss1, savedGame.airBoss1, savedGame.RunNumber, saveSlot);
        saveSlot += 1;
        if (saveSlot > 30) { saveSlot = 0; }

        savedGame = savedDataList[saveSlot];
        saveSlotPrefab4.SetupSaveSlotUI(savedGame.iceBoss1, savedGame.earthBoss1, savedGame.fireBoss1, savedGame.airBoss1, savedGame.RunNumber, saveSlot);
        saveSlot += 1;
        if (saveSlot > 30) { saveSlot = 0; }

        savedGame = savedDataList[saveSlot];
        saveSlotPrefab5.SetupSaveSlotUI(savedGame.iceBoss1, savedGame.earthBoss1, savedGame.fireBoss1, savedGame.airBoss1, savedGame.RunNumber, saveSlot);



    }

    private void LoadSavedDataToList(string saveName)
    {
        string appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Saves")) { Directory.CreateDirectory(appPath + "/Saves"); }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        GameSaverManager gameSaver = new GameSaverManager();

        if (!File.Exists(appPath + "/Saves/" + saveName + ".binary"))
        {
            FileStream saveFile2 = File.Create(appPath + "/Saves/" + saveName + ".binary");

            formatter.Serialize(saveFile2, gameSaver);
            saveFile2.Close();
        }

        saveFile = File.Open(appPath + "/Saves/" + saveName + ".binary", FileMode.Open);
        gameSaver = (GameSaverManager)formatter.Deserialize(saveFile);
        savedDataList.Add(gameSaver);
        saveFile.Close();

    }


    internal void AutoSave()
    {
        SaveGame(0);
        GameData.Instance.loadingFromDungeon = true;

    }

    internal void SaveGame(int saveSlot)
    {


        string appPath = Application.dataPath;
        string saveName;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        GameSaverManager gameSaver = new GameSaverManager();
        gameSaver.SetupSave();
        if (saveSlot == 0) saveName = "AutoSave";
        else saveName = "SaveSlot" + saveSlot;
        if (!Directory.Exists(appPath + "/Saves")) { Directory.CreateDirectory(appPath + "/Saves"); }
        if (!File.Exists(appPath + "/Saves/" + saveName + ".binary"))
        {
            saveFile = File.Create(appPath + "/Saves/" + saveName + ".binary");
            formatter.Serialize(saveFile, gameSaver);
            saveFile.Close();
        }
        else
        {
            saveFile = File.OpenWrite(appPath + "/Saves/" + saveName + ".binary");
            formatter.Serialize(saveFile, gameSaver);
            saveFile.Close();
        }
        savedDataList[saveSlot] = gameSaver;


    }

    public void SaveUiEntered(int slotEntered)
    {

        int slotOffset = slotEntered - pointLocation;
        if (slotOffset != 0) SoundManager.Instance.PlaySound("MenuMove", 1f);
        saveSlotSelected = saveSlotSelected + slotOffset;
        if (saveSlotSelected < 0)
        {
            saveSlotSelected = 31 + saveSlotSelected;
        }
        if (saveSlotSelected > 30)
        {
            saveSlotSelected = saveSlotSelected - 31;
        }
        pointLocation = slotEntered;
        saveSelector.GetComponent<RectTransform>().localPosition = new Vector3(-206, 125 - 80 * pointLocation, 0);
    }
    public void ClickedUI()
    {
        // SoundManager.Instance.PlaySound("MenuOkay", 1f);
        if (townSave)
        {
            //SaveSlotUiController tempUiSaveController = getUiSaveSlot();
            //tempUiSaveController.savingOverlay.SetActive(true);
            SaveGame(saveSlotSelected);
            SetupAllSaveUI(saveSlotSelected);
            //tempUiSaveController.savingOverlay.SetActive(false);
        }
        else
        {
            SetupLoadingGame();


        }
    }

    private void SetupLoadingGame()
    {
        GameSaverManager gameSaver = new GameSaverManager();
        gameSaver = savedDataList[saveSlotSelected];
        if (gameSaver.RunNumber == 0)
        {
            SoundManager.Instance.PlaySound("MenuNope", 1f);
            return;
        }

        if (!loadingGame)
        {

            loadingGame = true;
            SaveSlotUiController tempUiSaveController = getUiSaveSlot();
            tempUiSaveController.savingOverlay.SetActive(true);
            LoadGame(saveSlotSelected);
            //LoadGame(saveSlotSelected);
            //GameState.fullPause = false;
            //DeactivateLoad();
        }
    }

    protected SaveSlotUiController getUiSaveSlot()
    {
        SaveSlotUiController tempUiSaveController = null;
        switch (pointLocation)
        {
            case 0:
                tempUiSaveController = saveSlotPrefab1;
                break;
            case 1:
                tempUiSaveController = saveSlotPrefab2;
                break;
            case 2:
                tempUiSaveController = saveSlotPrefab3;
                break;
            case 3:
                tempUiSaveController = saveSlotPrefab4;
                break;
            case 4:
                tempUiSaveController = saveSlotPrefab5;
                break;
        }
        return tempUiSaveController;
    }

    public bool LoadGame(int saveSlot)
    {

            GameSaverManager gameSaver = new GameSaverManager();

            gameSaver = savedDataList[saveSlot];
        if (gameSaver.RunNumber == 0) {
            return false;
        }
            gameSaver.PushSaveToGameData();
        GameData.Instance.inDungeon = false;
        GameData.Instance.inPauseMenu = false;
        //aCanvasThis.enabled = false;
        newGameController.StartNewGameActual();
        GameState.setFullPause(false);
        return true;




    }

    internal void ActivateLoad(PauseMenuController callingScript, bool saveFile)
    {

        if (saveFile)
        {
            townSave = true;
            saveOrLoadText.text = "Save";
        }
        else
        {
            townLoad = true;
            saveOrLoadText.text = "Load";
        }
        pauseLoad = true;
        //   saveOrLoadText.text = "Load";
        callingPauseMenuControllerScriptReturn = callingScript;
        wait1Frame = false;
        loadSaveActive = true;
        aCanvasThis.enabled = true;
        saveSlotSelected = 0;
        pointLocation = 0;
        SetupAllSaveUI(saveSlotSelected);
    }

    public void ActivateLoad(NewGameController callingScript)
    {
        saveOrLoadText.text = "Load";
        callingGameControllerScriptReturn = callingScript;
        wait1Frame = false;
        loadSaveActive = true;
        aCanvasThis.enabled = true;
        saveSlotSelected = 0;
        pointLocation = 0;
        SetupAllSaveUI(saveSlotSelected);
    }

    internal void ActivateLoad(EscapeKeyController callingScript)
    {
        saveOrLoadText.text = "Load";
        callingEscapeKeyControllerScriptReturn = callingScript;
        wait1Frame = false;
        loadSaveActive = true;
        aCanvasThis.enabled = true;
        saveSlotSelected = 0;
        pointLocation = 0;
        SetupAllSaveUI(saveSlotSelected);
    }

    public void ActivateLoad(TownEscapeKeyController callingScript, bool saveFile)
    {
        Debug.Log("I should never get here. If so check town escape key ui is activate");
        if (saveFile)
        {
            townSave = true;
            saveOrLoadText.text = "Save";
        }
        else
        {
            townLoad = true;
            saveOrLoadText.text = "Load";
        }

        callingTownEscapeScriptReturn = callingScript;
        wait1Frame = false;
        loadSaveActive = true;
        aCanvasThis.enabled = true;
        saveSlotSelected = 0;
        pointLocation = 0;
        SetupAllSaveUI(saveSlotSelected);
    }

    public void DeactivateLoad()
    {
        wait1Frame = false;
        loadSaveActive = false;
        aCanvasThis.enabled = false;
        saveSlotSelected = 0;
        pointLocation = 0;

    }


}
