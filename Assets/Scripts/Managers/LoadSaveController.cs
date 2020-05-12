using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
//using UnityEngine.Windows;

public class LoadSaveController : MonoBehaviour
{

    public GameObject saveSlotPrefab;
    public GameObject itemContainer;
    public bool loadGame;
    public bool saveGame;
    internal static int saveSlotSelected = 0;
    internal static MetadataList metaListObject = new MetadataList();
    public Canvas aCanvasThis;
    public NewGameController newGameButton;

    // Start is called before the first frame update

    void Start()
    {
        GameData.Instance.FloorNumber = 0;


        if (!LoadMetaData()) {

            SaveMetadataObject autoSave = new SaveMetadataObject();
            autoSave.SaveName = "AutoSave";
            autoSave.runNumberText = "";
            autoSave.FurthestFloorReached = "Load Previous Game";
            metaListObject.listOfSaves.Add(autoSave);
            SaveMetaData();
        }
        GenerateSaveGamePrefabs();

    }

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
        else {
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

    

    private void GenerateSaveGamePrefabs()
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
            SaveMetadataObject autoSave = new SaveMetadataObject();
            autoSave.SaveName = "AutoSave";
            autoSave.runNumberText = "";
            autoSave.FurthestFloorReached = "Load New Game";
            metaListObject.listOfSaves.Add(autoSave);
            SaveMetaData();
        }
        

        SaveMetadataObject tempMeta = new SaveMetadataObject();
        tempMeta.SaveName = "Autosave";
        tempMeta.FurthestFloorReached = getTextForFloor(floorReached);
        tempMeta.runNumberText = getTextForDay(runNumber);
        updateMetaListData(0, tempMeta);
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

    public void addToMetalistData(SaveMetadataObject saveSlotObject) {
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
    public void canelLoad() {
        aCanvasThis.enabled = false;
    }
    public void activateLoad() {
        aCanvasThis.enabled = true ;
    }

}

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
