using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{

    [Header("Gameplay Screen")]
    [SerializeField] TextMeshProUGUI levelTitle;
    [SerializeField] GameObject fillBackground;
    [SerializeField] Image filledBar;

    [Header("Panels")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameplayScreen;
    [SerializeField] GameObject failScreen;
    [SerializeField] GameObject endGameScreen;

    public enum PanelType
    {
        Start,
        Gameplay,
        Fail,
        EndGame
    }
    private void Start()
    {
        SetLevelTitle();
        ToggleFillPanel(false);
    }
    public void SetLevelTitle() 
    {
        levelTitle.text = "Level " + (MainService.instance.dataService.currentLevel + 1)  ;
    }
    public void ChangePanel(PanelType type)
    {
        startScreen.SetActive(type == PanelType.Start);
        gameplayScreen.SetActive(type == PanelType.Gameplay);
        failScreen.SetActive(type == PanelType.Fail);
        endGameScreen.SetActive(type == PanelType.EndGame);
    }

    public void ToggleFillPanel(bool active)
    {
        fillBackground.SetActive(active);
        filledBar.fillAmount = 0;
    }
    public void SetFillPanel(float value)
    {
        filledBar.fillAmount = value;
    }
    public void Restart()
    {
        MainService.instance.gameplayService.Restart();
    }
    public void StartGame()
    {
        MainService.instance.gameplayService.StartGame();
    }
    public void NextLevel()
    {
        ChangePanel(PanelType.Start);
    }
}
