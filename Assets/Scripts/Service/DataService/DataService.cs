using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataService : MonoBehaviour
{
    public int currentLevel;
    public int resumeLevelIndex; //when levels are random. save last level
    public bool soundOn = true;
    public bool vibrationOn = true;
    private void Awake()
    {
        LoadData();
    }
    public void LoadData() //Load data from PlayerPrefs
    {
        soundOn = PlayerPrefs.GetInt("soundOn", 1) == 1;
        vibrationOn = PlayerPrefs.GetInt("vibrationOn", 1) == 1;
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        resumeLevelIndex = PlayerPrefs.GetInt("resumeLevelIndex", -1);
        Debug.Log("Data loaded successfuly");
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("resumeLevelIndex", resumeLevelIndex);
        PlayerPrefs.SetInt("soundOn", soundOn ? 1 : 0);
        PlayerPrefs.SetInt("vibrationOn", vibrationOn ? 1 : 0);
    }
    public void SetSound(bool soundBool)
    {
        soundOn = soundBool;
        PlayerPrefs.SetInt("soundOn", soundOn ? 1 : 0);
    }
    public void SetVibration(bool vibBool)
    {
        vibrationOn = vibBool;
        PlayerPrefs.SetInt("vibrationOn", vibrationOn ? 1 : 0);
    }
    private void OnApplicationPause(bool pause)
    {
        SaveData();
    }
    private void OnDisable()
    {
        SaveData();
    }
}
