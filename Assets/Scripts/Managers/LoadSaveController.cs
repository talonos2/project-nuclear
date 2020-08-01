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
    public  SaveSlotUiController saveSlotPrefab2;
    public  SaveSlotUiController saveSlotPrefab3;
    public  SaveSlotUiController saveSlotPrefab4;
    public  SaveSlotUiController saveSlotPrefab5;
    public Image saveSelector;
    public TextMeshProUGUI saveOrLoadText;
    //public GameObject itemContainer;
    //public bool loadGame;
    //public bool saveGame;
    internal int saveSlotSelected = 0;

    public Canvas aCanvasThis;
    public static List<GameSaverManager> savedDataList=new List<GameSaverManager>();
    //public NewGameController newGameButton;
    public bool loadSaveActive;
    private EscapeKeyController callingEscapeKeyControllerScriptReturn;
    private bool wait1Frame;
    private static int pointLocation = 0;
    private NewGameController callingGameControllerScriptReturn;
    private TownEscapeKeyController callingTownEscapeScriptReturn;
    private float selectionDelayDefault = .15f;
    private float selectionDelay = 0;
    private float speedupTimeDefault=1.25f;
    private float speedupTimer;
    private bool townSave;
    private bool townLoad;
    private bool dungeonLoad;

    // Start is called before the first frame update

    void Start()
    {

        LoadSavedDataToList("Autosave");
        for (int i = 1; i < 31; i++ ) {
            LoadSavedDataToList("SaveSlot"+i);
        }

        SetupAllSaveUI(saveSlotSelected);
        aCanvasThis.enabled = false;



    }

    private void Update()
    {
        if (!loadSaveActive) return;
        if (!wait1Frame) {wait1Frame = true; return; }

        if (Input.GetButtonDown("Submit")) {
            SoundManager.Instance.PlaySound("MenuOkay", 1f);
            if (townSave) {
                //SaveSlotUiController tempUiSaveController = getUiSaveSlot();
                //tempUiSaveController.savingOverlay.SetActive(true);
                SaveGame(saveSlotSelected);
                SetupAllSaveUI(saveSlotSelected);
                //tempUiSaveController.savingOverlay.SetActive(false);
            }
            else
            {
                LoadGame(saveSlotSelected);
                GameState.fullPause = false;
                deactivateLoad();
            }

        }       

        if (Input.GetButtonDown("SelectUp"))
        {
            selectionDelay = selectionDelayDefault+.4f;
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

        if (Input.GetButtonDown("SelectDown"))
        {
            selectionDelay = selectionDelayDefault+.4f;
            speedupTimer = speedupTimeDefault;
            saveSlotSelected += 1;
            if (saveSlotSelected >30 )
                saveSlotSelected = 0;
            SoundManager.Instance.PlaySound("MenuMove", 1f);
            pointLocation++;
            if (pointLocation > 4)
                pointLocation = 4;
            SetupAllSaveUI(saveSlotSelected);
        }

        if (Input.GetAxisRaw("SelectUp") > .1f)
        {
            selectionDelay -= Time.deltaTime;
            speedupTimer -= Time.deltaTime;
            if (speedupTimer <= 0) selectionDelay -= Time.deltaTime / 2.5f;
            if (speedupTimer <= -.75f) selectionDelay -= Time.deltaTime / 1.75f;
            if (selectionDelay <= 0)
            {
                saveSlotSelected -= 1;
                if (saveSlotSelected < 0)
                    saveSlotSelected = 30;
                SoundManager.Instance.PlaySound("MenuMove", 1f);
                pointLocation--;
                if (pointLocation < 0)
                    pointLocation = 0;
                SetupAllSaveUI(saveSlotSelected);
                selectionDelay = selectionDelayDefault;
            }

        }
        else if (Input.GetAxisRaw("SelectDown") > .1f)
        {
            selectionDelay -= Time.deltaTime;
            speedupTimer -= Time.deltaTime;
            if (speedupTimer <= 0) selectionDelay -= Time.deltaTime / 2.5f;
            if (speedupTimer <= -.75f) selectionDelay -= Time.deltaTime / 1.75f;
            if (selectionDelay <= 0)
            {
                saveSlotSelected += 1;
                if (saveSlotSelected > 30)
                    saveSlotSelected = 0;
                SoundManager.Instance.PlaySound("MenuMove", 1f);
                pointLocation++;
                if (pointLocation > 4)
                    pointLocation = 4;
                SetupAllSaveUI(saveSlotSelected);
                selectionDelay = selectionDelayDefault;
            }

        }


        if (Input.GetButtonDown("Cancel")) {
            deactivateLoad();

            if (townSave || townLoad) callingTownEscapeScriptReturn.ReActivate();
            else if (dungeonLoad) callingEscapeKeyControllerScriptReturn.ReActivate();
            else callingGameControllerScriptReturn.ReActivate();
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
            saveFile = File.Create(appPath + "/Saves/"+ saveName + ".binary");

            formatter.Serialize(saveFile, gameSaver);
            saveFile.Close();
        }

        saveFile = File.Open(appPath + "/Saves/" + saveName + ".binary", FileMode.Open);
        gameSaver = (GameSaverManager)formatter.Deserialize(saveFile);
        savedDataList.Add(gameSaver);
        saveFile.Close();

    }


    internal void autoSave()
    {
        SaveGame(0);

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
        else {
            saveFile = File.OpenWrite(appPath + "/Saves/" + saveName + ".binary");
            formatter.Serialize(saveFile, gameSaver);
            saveFile.Close();
        }
        savedDataList[saveSlot]= gameSaver;


    }

    protected SaveSlotUiController getUiSaveSlot()
    {
        SaveSlotUiController tempUiSaveController=null;
        switch (pointLocation) {
            case 0:
                //tempUiSaveController = saveSlotPrefab1;
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

    public void LoadGame(int saveSlot)
    {
        GameSaverManager gameSaver = new GameSaverManager();
        
        gameSaver= savedDataList[saveSlot];
        gameSaver.PushSaveToGameData();

        aCanvasThis.enabled = false;
        //newGameButton.StartNewGameActual();
        NewGameController.StartNewGameActual();
        

    }

    public void activateLoad(NewGameController callingScript)
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

    internal void activateLoad(EscapeKeyController callingScript)
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

    public void activateLoad(TownEscapeKeyController callingScript, bool saveFile)
    {
        if (saveFile) { townSave = true;
            saveOrLoadText.text = "Save";
        }
        else { townLoad = true;
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

    public void deactivateLoad() {
        wait1Frame = false;
        loadSaveActive = false;
        aCanvasThis.enabled = false;
        saveSlotSelected = 0;
        pointLocation = 0;
    }


}


/*Old void Start(){

*if (!LoadMetaData()) {
 * 
 *

           SaveMetadataObject autoSave = new SaveMetadataObject();
           autoSave.SaveName = "AutoSave";
           autoSave.runNumberText = "";
           autoSave.FurthestFloorReached = "Load Previous Game";
           metaListObject.listOfSaves.Add(autoSave);
           SaveMetaData();
       }
       GenerateSaveGamePrefabs();}


/*
public static void SaveGame() {
    String appPath = Application.dataPath;
    if (!Directory.Exists(appPath + "/Saves"))
        Directory.CreateDirectory(appPath + "/Saves");

    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile;
    if (File.Exists(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary"))
    {
        saveFile = File.OpenWrite(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary");
    }
    else
    {
        saveFile = File.Create(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary");
    }

    GameSaverManager gameSaver = new GameSaverManager();
    gameSaver.SetupSave();

    formatter.Serialize(saveFile, gameSaver);

    saveFile.Close();

}
public bool LoadGame() {
    string appPath = Application.dataPath;
    if (!Directory.Exists(appPath + "/Saves")) { return false; }
    if (!File.Exists(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary"))
    {
        return false;
    }
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile;
        saveFile = File.Open(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary", FileMode.Open);
        GameSaverManager gameSaver = (GameSaverManager)formatter.Deserialize(saveFile);
    if (gameSaver.RunNumber == 0)
    {
        return false;
    }
        gameSaver.PushSaveToGameData();

        saveFile.Close();
        return true;



}



/*private void GenerateSaveGamePrefabs()
{
    foreach (SaveMetadataObject o in metaListObject.listOfSaves) {
        GameObject listItem = Instantiate(saveSlotPrefab) as GameObject;
        SaveSlotController cntrl = listItem.GetComponent<SaveSlotController>();
        cntrl.SaveName.text = o.SaveName;
        cntrl.furtherstTraveled.text = o.FurthestFloorReached;
        cntrl.currentDate.text = o.runNumberText;
        listItem.SetActive(true);
        listItem.transform.SetParent(itemContainer.transform, false);
    }
}

internal static void autoSave(int runNumber, int floorReached)
{
    if (metaListObject.listOfSaves.Count == 0) {
        Debug.Log("New AutoSave");
        //SaveMetadataObject autoSave = new SaveMetadataObject();
        //autoSave.SaveName = "AutoSave";
        //autoSave.runNumberText = "";
        //autoSave.FurthestFloorReached = "Load New Game";
        metaListObject.listOfSaves.Add(autoSave);
      //  SaveMetaData();
    }


    //SaveMetadataObject tempMeta = new SaveMetadataObject();
    //tempMeta.SaveName = "Autosave";
    //tempMeta.FurthestFloorReached = getTextForFloor(floorReached);
    //tempMeta.runNumberText = getTextForDay(runNumber);
    //updateMetaListData(0, tempMeta);
    SaveGame();

}

private static string getTextForDay(int runNumber)
{
    string answer = "Day " + runNumber;
    return answer;
}

private static string getTextForFloor(int runNumber)
{
    string answer = "Farthest Floor Delved \nFloor" + runNumber;
    return answer;
}

/* public void addToMetalistData(SaveMetadataObject saveSlotObject) {
    metaListObject.listOfSaves.Add(saveSlotObject);
}

private static void updateMetaListData(int saveSlot, SaveMetadataObject metaDataToSave) {
    LoadMetaData();
    metaListObject.listOfSaves[saveSlot] = metaDataToSave;

    SaveMetaData();
}

private static void SaveMetaData()
{
    String appPath = Application.dataPath;
    if (!Directory.Exists(appPath+"/Saves"))
        Directory.CreateDirectory(appPath + "/Saves");

    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile = File.Create(appPath+"/Saves/MetaData.binary");

    //LocalCopyOfData = PlayerState.Instance.localPlayerData;

    formatter.Serialize(saveFile, metaListObject);

    saveFile.Close();
}

private static bool LoadMetaData()
{
    string appPath = Application.dataPath;
    if (!Directory.Exists(appPath + "/Saves")) { return false; }
    if (!File.Exists(appPath + "/Saves/MetaData.binary")) {
        return false;
    }
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile = File.Open(appPath+"/Saves/MetaData.binary", FileMode.Open);
    metaListObject.listOfSaves.Clear();
    MetadataList tempMetaListObject = (MetadataList)formatter.Deserialize(saveFile);
    metaListObject.listOfSaves = tempMetaListObject.listOfSaves;

    saveFile.Close();
    return true;
}

public void LoadGameAndStart() {
    if (!LoadGame()) {
        Debug.Log("Failed to Load Game");
    }
    newGameButton.StartNewGameActual();

}
public void CancelLoad() {
    aCanvasThis.enabled = false;
}
public void activateLoad() {
    aCanvasThis.enabled = true ;
}

}
/*
[Serializable]
public class MetadataList {
public List<SaveMetadataObject> listOfSaves=new List<SaveMetadataObject>();
}

[Serializable]
public class SaveMetadataObject
{
public String SaveName;
public String runNumberText;
public String FurthestFloorReached;


}
*/
