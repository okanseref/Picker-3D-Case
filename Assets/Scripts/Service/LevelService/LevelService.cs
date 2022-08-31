using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelService : MonoBehaviour
{
    public GameObject endGamePrefab;
    public GameObject[] levels;
    float currentZ = 0;
    GameObject endGame;
    public GameObject currentLevel,nextLevel;
    List<GameplayState> stateObjectList;
    const float rampDistance = 128.5f;
    private void Awake()
    {
        levels = Resources.LoadAll("Prefabs/Levels", typeof(GameObject)).Cast<GameObject>().ToArray();
    }
    void Start()
    {
        endGame = Instantiate(endGamePrefab);
        currentZ = 0;
        CreateLevel();
        CreateNextLevel();
    }
    public void StartLevel()
    {
        //stateObjectList = new List<GameplayState>();
        //FindStateObjects(); 
    }
    //private void FindStateObjects()//Objects that has something to do when game is paused
    //{
    //    stateObjectList.Clear();
    //    var stateObjects = FindObjectsOfType<MonoBehaviour>().OfType<GameplayState>();
    //    foreach (GameplayState stateObject in stateObjects)
    //    {
    //        stateObjectList.Add(stateObject);
    //    }
    //}
    private void CreateLevel()
    {
        currentLevel = Instantiate(levels[GetLevelIndex(MainService.instance.dataService.currentLevel)]);
        int levelLength = currentLevel.GetComponent<Level>().levelLength;
        currentLevel.transform.position = new Vector3(0, 0, currentZ);
        endGame.transform.position = new Vector3(0, 0, levelLength * 5 + currentZ);
        currentZ += (levelLength * 7)+rampDistance;
    }
    private void CreateNextLevel()
    {
        nextLevel = Instantiate(levels[GetLevelIndex(MainService.instance.dataService.currentLevel+1)]);
        int levelLength = nextLevel.GetComponent<Level>().levelLength;
        nextLevel.transform.position = new Vector3(0, 0, currentZ);
        //currentZ += levelLength * 7;
        //carry end game
    }
    public void LevelFinished()
    {
        Destroy(currentLevel);
        MainService.instance.dataService.currentLevel++;
        currentLevel = nextLevel;
        int levelLength = currentLevel.GetComponent<Level>().levelLength;
        endGame.transform.position = new Vector3(0, 0, levelLength * 5 + currentZ);
        currentZ += rampDistance;
        CreateNextLevel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            LevelFinished();
        }
    }
    private int GetLevelIndex(int levelNo)
    {
        if (levelNo >= levels.Length)
        {
            return Random.Range(0, levels.Length);
        }
        else
        {
            return levelNo;
        }
    }
}
