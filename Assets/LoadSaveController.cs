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
    // Start is called before the first frame update

    void Start()
    {

        if (!LoadMetaData()) {
            SaveMetadataObject autoSave = new SaveMetadataObject();
            autoSave.SaveName = "AutoSave";
            autoSave.runNumberText = "";
            autoSave.FurthestFloorReached = "No Saved Progress Yet";
            metaListObject.listOfSaves.Add(autoSave);
            SaveMetaData();
        }
        GenerateSaveGamePrefabs();

    }

    public void SaveGame() {
        String appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Saves"))
            Directory.CreateDirectory(appPath + "/Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary");

        formatter.Serialize(saveFile, GameData.Instance);

        saveFile.Close();

    }
    public bool LoadGame() {
        String appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Saves")) { return false; }
        if (!File.Exists(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary"))
        {
            return false;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(appPath + "/Saves/" + metaListObject.listOfSaves[saveSlotSelected].SaveName + ".binary", FileMode.Open);

        GameData loadedData = (GameData)formatter.Deserialize(saveFile);
        GameData.Instance.updateFields(loadedData);
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
        SaveMetadataObject tempMeta = new SaveMetadataObject();
        tempMeta.SaveName = "Autosave";
        tempMeta.FurthestFloorReached = getTextForFloor(floorReached);
        tempMeta.runNumberText = getTextForDay(runNumber);
        updateMetaListData(0, tempMeta);

    }

    private static string getTextForDay(int runNumber)
    {
        return "Day "+runNumber;
    }

    private static string getTextForFloor(int runNumber)
    {
        return "Farthest Floor Delved /nFloor" + runNumber;
    }

    public void addToMetalistData(SaveMetadataObject saveSlotObject) {
        metaListObject.listOfSaves.Add(saveSlotObject);
    }

    private static void updateMetaListData(int saveSlot, SaveMetadataObject metaDataToSave) {
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

    private bool LoadMetaData()
    {
        String appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Saves")) { return false; }
        if (!File.Exists(appPath + "Saves/MetaData.binary")) {
            return false;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(appPath+"Saves/MetaData.binary", FileMode.Open);

        metaListObject = (MetadataList)formatter.Deserialize(saveFile);

        saveFile.Close();
        return true;
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
