using Cinemachine;
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
    const float rampDistance = 125f;
    int currentLevelIndex, nextLevelIndex;
    private void Awake()
    {
        levels = Resources.LoadAll("Prefabs/Levels", typeof(GameObject)).Cast<GameObject>().ToArray();
    }
    void Start()
    {
        endGame = Instantiate(endGamePrefab);
        currentZ = 0;
        StartCoroutine(CreateLevel());
        //CreateNextLevel();
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
    IEnumerator CreateLevel()
    {
        currentLevelIndex = GetLevelIndex(MainService.instance.dataService.currentLevel,true);
        currentLevel = Instantiate(levels[currentLevelIndex]);
        currentLevel.SetActive(false);
        yield return new WaitUntil(() => currentLevel != null);
        currentLevel.SetActive(true);
        Level levelScript = currentLevel.GetComponent<Level>();
        int levelLength = levelScript.levelLength;
        levelScript.ReleaseRigidbodies();
        currentLevel.transform.position = new Vector3(0, 0, currentZ);
        print(levelLength+" "+levelScript.levelLength+levelScript.gameObject.name+ currentLevel.name);
        endGame.transform.position = new Vector3(0, 0, (levelLength - 1) * 7 + currentZ);
        currentZ += (levelLength * 7)+rampDistance;
        StartCoroutine(CreateNextLevel());

    }
    IEnumerator CreateNextLevel()
    {
        nextLevelIndex = GetLevelIndex(MainService.instance.dataService.currentLevel + 1,false);
        nextLevel = Instantiate(levels[nextLevelIndex]);
        nextLevel.SetActive(false);
        yield return new WaitUntil(() => nextLevel != null);
        nextLevel.SetActive(true);
        Level levelScript = nextLevel.GetComponent<Level>();
        levelScript.ReleaseRigidbodies(); 
        nextLevel.transform.position = new Vector3(0, 0, currentZ);
        //currentZ += levelLength * 7;
        //carry end game
    }
    public IEnumerator RestartLevel()
    {
        Vector3 pos = currentLevel.transform.position;
        Destroy(currentLevel);
        currentLevel = Instantiate(levels[currentLevelIndex]);
        currentLevel.transform.position = pos;
        MainService.instance.gameplayService.ResetPlayerPos();
        // prevent virtual camera glitching
        MainService.instance.gameplayService.vcam.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
        yield return new WaitForSeconds(0.2F);
        Level levelScript = currentLevel.GetComponent<Level>();
        levelScript.ReleaseRigidbodies();
        MainService.instance.gameplayService.vcam.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

    }
    public void LevelFinished()
    {
        Destroy(currentLevel);
        MainService.instance.dataService.currentLevel++;
        currentLevel = nextLevel;
        currentLevelIndex = nextLevelIndex;
        MainService.instance.dataService.resumeLevelIndex = currentLevelIndex;
        int levelLength = currentLevel.GetComponent<Level>().levelLength;
        endGame.transform.position = new Vector3(0, 0, (levelLength-1) * 7 + currentZ);
        currentZ += rampDistance+ levelLength * 7;
        StartCoroutine(CreateNextLevel());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            LevelFinished();
        }
    }
    private int GetLevelIndex(int levelNo,bool isFirstLevel)
    {
        if (levelNo >= levels.Length)
        {
            if (MainService.instance.dataService.resumeLevelIndex != -1&& isFirstLevel)
            {
                return MainService.instance.dataService.resumeLevelIndex;
            }
            else
            {
                return Random.Range(0, levels.Length);
            }
        }
        else
        {
            return levelNo;
        }
    }
}
