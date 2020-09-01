using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public static AudioClip potBreakSound, rockAttackStrongSound, rockAttackWeakSound;
    static AudioSource audioSrc;
    AudioSource environmentalSound;
    internal string currentlyPlayingEnvTrack = "";
    private DictionaryOfStringAndFloat soundVolumeMap = new DictionaryOfStringAndFloat();
    private bool loadedJson = false;

    public float soundEffectVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void LoadSoundJSON()
    {
        String appPath = Application.dataPath;
        if (!Directory.Exists(appPath + "/Sound"))
        {
            Directory.CreateDirectory(appPath + "/Sound");
        }

       // Debug.Log("Here, checking.");
        if (File.Exists(appPath + "/Sound/soundVolumeMap.json"))
        {
            //Debug.Log("Found, Loading.");
            StreamReader reader = new StreamReader(appPath+"/Sound/soundVolumeMap.json");
            String json = reader.ReadToEnd();
            //Debug.Log("Loaded: " + json);
            soundVolumeMap = JsonUtility.FromJson<DictionaryOfStringAndFloat>(json);
           // foreach (string s in soundVolumeMap.Keys)
           //{
           //     Debug.Log("Loaded: "+s + ", " + soundVolumeMap[s]);
           // }
            reader.Close();
        }
        else
        {
            //Debug.Log("Not found, creating.");
            File.Create(appPath + "/Sound/soundVolumeMap.json");
        }
    }

    private float SafeGetVolume(string s)
    {
        if (!loadedJson)
        {
            LoadSoundJSON();
            loadedJson = true;
        }
        if (!soundVolumeMap.ContainsKey(s))
        {
            Debug.Log("Does not contain " + s);
            soundVolumeMap.Add(s, 1f);
            SaveSoundMap();
        }
        return soundVolumeMap[s];
    }

    private void SaveSoundMap()
    {
        String appPath = Application.dataPath;
        StreamWriter writer = new StreamWriter(appPath + "/Sound/soundVolumeMap.json", false);
        //foreach (string s in soundVolumeMap.Keys)
        //{
        //    Debug.Log(s+", "+soundVolumeMap[s]);
        //}
        String toPrint = JsonUtility.ToJson(soundVolumeMap, true);
        //Debug.Log(toPrint);
        writer.Write(toPrint);
        writer.Flush();
        writer.Close();
    }

    private void CreateEnvironmentalSound()
    {
        GameObject tempGO = new GameObject();
        tempGO.name = "EnvironmentalSound";
        tempGO.transform.parent = this.transform;
        environmentalSound = tempGO.AddComponent<AudioSource>();
        environmentalSound = tempGO.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string clip, float vol)
    {
        float realVol = vol * SafeGetVolume(clip)*soundEffectVolume;
        if (!audioSrc)
        {
            audioSrc = this.gameObject.AddComponent<AudioSource>();
        }
        audioSrc.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + clip), realVol);
    }

    public void PlayPersistentSound(string clip, float vol)
    {
        float realVol = vol * SafeGetVolume(clip)*soundEffectVolume;
        audioSrc.clip = Resources.Load<AudioClip>("Sounds/" + clip);
        audioSrc.volume = realVol;
        audioSrc.Play();
    }

    public void ChangeEnvironmentTrack(string clip)
    {
        if (!environmentalSound)
        {
            CreateEnvironmentalSound();
        }
        environmentalSound.clip = GetAudio("Sounds/Environment/" + clip);
        environmentalSound.loop = true;
        currentlyPlayingEnvTrack = clip;
        environmentalSound.Play();
    }

    private AudioClip GetAudio(string v)
    {
            return Resources.Load<AudioClip>(v);
    }

    public void ChangeEnvironmentTrack()
    {
        if (!environmentalSound)
        {
            CreateEnvironmentalSound();
        }
        if (!environmentalSound)
        {
            CreateEnvironmentalSound();
        }
        currentlyPlayingEnvTrack = "";
        if (environmentalSound)
        {
            environmentalSound.Stop();
        }
    }

    public void ChangeEnvironmentVolume(float vol)
    {
        if (!environmentalSound)
        {
            CreateEnvironmentalSound();
        }
        if (environmentalSound.clip)
        {
            float realVol = vol * SafeGetVolume(environmentalSound.clip.name)*soundEffectVolume;
            environmentalSound.volume = realVol / 9.0f;
        }
        else
        {
            environmentalSound.volume = 0;
        }
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }

    private void OnApplicationQuit()
    {
        if (!Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }

    [Serializable] public class DictionaryOfStringAndFloat : SerializableDictionary<string, float> { }
}
