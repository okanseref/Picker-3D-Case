using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayService : MonoBehaviour
{
    public PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Idle()
    {
        playerController.collisionList.Clear();
        playerController.transform.position = MainService.instance.levelService.currentLevel.transform.position;
        MainService.instance.uiService.ChangePanel(UIService.PanelType.Start);
        playerController.SwitchMode(PlayerController.Mode.None);
        playerController.ResetPlayer();
    }
    public void StartGame()
    {
        MainService.instance.uiService.ChangePanel(UIService.PanelType.Gameplay);
        MainService.instance.uiService.SetLevelTitle();
        playerController.SwitchMode(PlayerController.Mode.Normal);

    }
    public void Restart()
    {
        playerController.collisionList.Clear();
        playerController.transform.position = MainService.instance.levelService.currentLevel.transform.position;
        MainService.instance.uiService.ChangePanel(UIService.PanelType.Start);
    }
    public void NextLevel()
    {
        MainService.instance.levelService.LevelFinished();
        playerController.collisionList.Clear();
        playerController.transform.position = MainService.instance.levelService.currentLevel.transform.position;
        //MainService.instance.uiService.ChangePanel(UIService.PanelType.Start);
    }
}
