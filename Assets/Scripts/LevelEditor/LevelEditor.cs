using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject emptyLevelPrefab;
    public GameObject[] levelList;
    public Button saveButton;
    public Dropdown dropdown;
    public CinemachineVirtualCamera vcam;
    public GameObject currentLevel;
    public TextMeshProUGUI gridModeText;
    public TextMeshProUGUI clickModeText;
    Level currentLevelScript;
    GameObject selectedPrefab;

    [Header("Complete Count Menu")]
    public InputField completeCountInput;
    public GameObject completeCountMenu;
    public PitController selectedPit;

    [Header("Grounds")]
    public GameObject ground_simple;
    public GameObject ground_pit;

    [Header("Objects")]
    public GameObject[] objectList;

    public ClickMode clickMode = ClickMode.None;
    public bool gridMode = false;
    public enum ClickMode
    {
        Create,
        Delete,
        None
    }
    private void Awake()
    {
        levelList = Resources.LoadAll("Prefabs/Levels", typeof(GameObject)).Cast<GameObject>().ToArray();
        dropdown.ClearOptions();
        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        foreach (GameObject item in levelList)
        {
            list.Add(new Dropdown.OptionData(item.name));
        }
        dropdown.AddOptions(list);
    }
    private void Start()
    {
        //Physics.autoSimulation = false;

    }
    private void ClearScene()
    {
        Destroy(currentLevel);
    }
    public void NewLevel()
    {
        ClearScene();
        //empty
        currentLevel = Instantiate(emptyLevelPrefab);
        currentLevel.transform.position = Vector3.zero;
        currentLevelScript = currentLevel.GetComponent<Level>();
    }
    public void LoadLevel()
    {
        currentLevel= Instantiate(levelList[dropdown.value]);
        currentLevelScript = currentLevel.GetComponent<Level>();
    }
    public void SaveLevel()
    {
        string localPath = "Assets/Resources/Prefabs/Levels/" + currentLevel.name + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAssetAndConnect(currentLevel, localPath, InteractionMode.UserAction, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200.0f))
            {
                if(hit.transform.tag.Equals("Ground")&& clickMode==ClickMode.Create)
                {
                    CreateObject(hit.point);
                }
                else if(IsObject(hit.transform.tag) && clickMode == ClickMode.Delete)
                {
                    Destroy(hit.transform.gameObject);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }
    public void CameraMove(float value)
    {
        vcam.transform.position = new Vector3(vcam.transform.position.x, vcam.transform.position.y, vcam.transform.position.z + value);
    }
    public void AddGround(bool isSimple)
    {
        if (currentLevel == null) { return; }
        GameObject ground;
        if (isSimple)
        {
            ground = Instantiate(ground_simple);
        }
        else
        {
            ground = Instantiate(ground_pit);
        }
        ground.transform.SetParent(currentLevel.transform.GetChild(0));
        ground.transform.position = new Vector3(0,0,currentLevelScript.levelLength * 7);
        currentLevelScript.levelLength++;
    }
    public void RemoveGround()
    {
        if (currentLevel == null) { return; }

        if (currentLevelScript.levelLength > 0)
        {
            Destroy(currentLevel.transform.GetChild(0).GetChild(currentLevelScript.levelLength - 1).gameObject);
            currentLevelScript.levelLength--;
        }
    }
    private bool IsObject(string tag)
    {
        return tag.Equals("Ball") || tag.Equals("BigBall") || tag.Equals("PowerUp");
    }
    public void ToggleGridMode()
    {
        if (gridMode)
        {
            gridModeText.text = "Grid Mode: OFF";
            gridMode = false;
        }
        else
        {
            gridModeText.text = "Grid Mode: ON";
            gridMode = true;
        }
    }
    private void CreateObject(Vector3 position)
    {
        if (currentLevel == null) { return; }
        GameObject createdObject = Instantiate(selectedPrefab);
        createdObject.transform.SetParent(currentLevel.transform.GetChild(1));
        if (gridMode)
        {
            int valX = (int)(position.x / 0.5f);
            if(Mathf.Abs(0.5f * valX - position.x) < Mathf.Abs(0.5f * (valX + 1) - position.x))
            {
                position.x = 0.5f * valX;
            }
            else
            {
                position.x = 0.5f * (valX + 1);
            }
            int valZ = (int)(position.z / 0.5f);
            if (Mathf.Abs(0.5f * valZ - position.z) < Mathf.Abs(0.5f * (valZ + 1) - position.z))
            {
                position.z = 0.5f * valZ;
            }
            else
            {
                position.z = 0.5f * (valZ + 1);
            }
        }
        createdObject.transform.position = new Vector3(position.x,position.y+0.5f,position.z);
        if (createdObject.GetComponent<Rigidbody>() != null)
        {
            createdObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    public void AddObject(int index)
    {
        if (currentLevel == null) { return; }
        ChangeClickMode(ClickMode.Create);
        selectedPrefab = objectList[Mathf.Clamp(index,0,objectList.Length)];
    }
    public void CancelCreateMode()
    {
        ChangeClickMode(ClickMode.None);
    }
    public void RemoveObject()
    {
        ChangeClickMode(ClickMode.Delete);
    }
    public void ClearAllObjects()
    {
        foreach (Transform child in currentLevel.transform.GetChild(1).transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void ChangeClickMode(ClickMode mode)
    {
        clickMode = mode;
        clickModeText.text = "Click Mode: " + mode.ToString();
    }
}
