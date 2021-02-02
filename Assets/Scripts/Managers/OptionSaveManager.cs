using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistentSaveDataManager : Singleton<PersistentSaveDataManager>
{
    [Serializable]
    private class SerializablePersistentSaveData
    {
        internal float musicVolume = 1;
        internal float soundVolume = 1;

        internal bool[,] endingsSeen = new bool[30, 3];
        internal bool[] peopleWonWith = new bool[30];
    }

    SerializablePersistentSaveData data;

    public float MusicVolume { get { CheckData(); return data.musicVolume; } set { CheckData(); data.musicVolume = value; MusicManager.instance.ChangeMusicVolume(value); FlushSavesToDisk(); } }
    public float SoundVolume { get { CheckData(); return data.soundVolume; } set { CheckData(); data.soundVolume = value; SoundManager.Instance.soundEffectVolume = value; FlushSavesToDisk(); } }
    public bool[,] EndingsSeen { get { CheckData(); return data.endingsSeen; } set { CheckData(); data.endingsSeen = value; FlushSavesToDisk(); } }
    public bool[] PeopleWonWith { get { CheckData(); return data.peopleWonWith; } set { CheckData(); data.peopleWonWith = value; FlushSavesToDisk(); } }

    public void Start()
    {
    }

    private void CheckData()
    {
        if (data==null)
        {
            LoadSavedOptionData();
        }
    }

    private void FlushSavesToDisk()
    {
       SaveOptions();
    }

    internal void SaveOptions()
    {

        string appPath = Application.dataPath;
        string savedOptions = "GameOptions";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;

        if (!Directory.Exists(appPath + "/Saves")) { Directory.CreateDirectory(appPath + "/Saves"); }
        if (!File.Exists(appPath + "/Saves/" + savedOptions + ".binary"))
        {
            saveFile = File.Create(appPath + "/Saves/" + savedOptions + ".binary");
            formatter.Serialize(saveFile, data);
            saveFile.Close();
        }
        else
        {
            saveFile = File.OpenWrite(appPath + "/Saves/" + savedOptions + ".binary");
            formatter.Serialize(saveFile, data);
            saveFile.Close();
        }

        saveFile.Close();
    }

    internal void LoadSavedOptionData()
    {
        string appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Saves")) { Directory.CreateDirectory(appPath + "/Saves"); }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile;
        string savedOptions = "GameOptions";

        if (!File.Exists(appPath + "/Saves/" + savedOptions + ".binary"))
        {

            FileStream saveFile2 = File.Create(appPath + "/Saves/" + savedOptions + ".binary");
            if (data == null)
            {
                data = new SerializablePersistentSaveData();
            }
            formatter.Serialize(saveFile2, data);
            saveFile2.Close();
        }

        saveFile = File.Open(appPath + "/Saves/" + savedOptions + ".binary", FileMode.Open);
        data = ((SerializablePersistentSaveData)formatter.Deserialize(saveFile));
        OptionScreenController options = GameObject.FindObjectOfType<OptionScreenController>();
        saveFile.Close();

        MusicManager.instance.ChangeMusicVolume(PersistentSaveDataManager.Instance.MusicVolume);
        SoundManager.Instance.soundEffectVolume = PersistentSaveDataManager.Instance.SoundVolume;

        saveFile.Close();
    }
}