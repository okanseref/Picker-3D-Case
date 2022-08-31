using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainService : MonoBehaviour
{
    public static MainService instance = null;
    public static void SetMainService(MainService HCService)
    {
        instance = HCService;
    }
    public DataService dataService;
    public UIService uiService;
    public SoundService soundService;
    public LevelService levelService;
    public PoolService poolService;
    public GameplayService gameplayService;

    public void SetService()
    {
        if (MainService.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        MainService.SetMainService(this);
    }
    void Awake()
    {
        SetService();
        DontDestroyOnLoad(this);
    }
}
