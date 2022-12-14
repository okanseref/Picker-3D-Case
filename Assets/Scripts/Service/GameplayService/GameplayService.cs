using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayService : MonoBehaviour
{
    public PlayerController playerController;
    public CinemachineVirtualCamera vcam;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Idle() //Waiting player
    {
        playerController.collisionList.Clear();
        ResetPlayerPos();
        MainService.instance.uiService.ChangePanel(UIService.PanelType.Start);
        playerController.SwitchMode(PlayerController.Mode.None);
        playerController.ResetPlayer();
    }
    public void StartGame() //Start running
    {
        MainService.instance.uiService.ChangePanel(UIService.PanelType.Gameplay);
        MainService.instance.uiService.SetLevelTitle();
        playerController.SwitchMode(PlayerController.Mode.Normal);

    }
    public void Restart()//Restart current level only running
    {
        print("Restart");
        playerController.collisionList.Clear();
        playerController.StopSpeed();
        StartCoroutine(MainService.instance.levelService.RestartLevel());

        MainService.instance.uiService.ChangePanel(UIService.PanelType.Start);
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Ball"))
        {
            if (item.transform.parent == null)
            {
                MainService.instance.poolService.PushObject(PoolService.PoolType.ball,item);
            }
        }
    }
    public void NextLevel()
    {
        MainService.instance.levelService.LevelFinished();
        playerController.collisionList.Clear();
        ResetPlayerPos();
    }
    public void ResetPlayerPos()
    {
        Vector3 movePosition = MainService.instance.levelService.currentLevel.transform.position;
        movePosition.y += 0.2f;
        playerController.GetComponent<Rigidbody>().position = movePosition;
        playerController.transform.rotation = Quaternion.identity;
        playerController.ResetPlayer();
    }
}
