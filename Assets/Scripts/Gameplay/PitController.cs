using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PitController : MonoBehaviour
{
    public int required_balls = 10;
    public TextMeshPro countText;
    public GameObject ground_under;
    public List<GameObject> balls;
    bool isCompleted = false;
    bool failed = false;
    Sequence loseSequence;
    // Start is called before the first frame update
    void Start()
    {
        balls =new List<GameObject>();
        RefreshText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Completed()
    {
        //loseSequence.Pause();
        print(DOTween.Pause("loseTimer"));
       print( DOTween.Kill("loseTimer"));
        List<GameObject> fxList=new List<GameObject>();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            MainService.instance.soundService.PlaySound(1, 1f);
            foreach (GameObject ball in balls)
            {
                MainService.instance.poolService.PushObject(PoolService.PoolType.ball, ball);

                GameObject breakFX =MainService.instance.poolService.PopObject(PoolService.PoolType.breakFX);
                breakFX.SetActive(true);
                breakFX.GetComponent<ParticleSystem>().Play();
                breakFX.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y+0.5f, ball.transform.position.z);
                fxList.Add(breakFX);
            }
        });
        seq.AppendInterval(1.6f);
        seq.AppendCallback(() =>
        {
            foreach (GameObject breakFX in fxList)
            {
                MainService.instance.poolService.PushObject(PoolService.PoolType.breakFX,breakFX);
            }
        });
        seq.Append(ground_under.transform.DOLocalMoveY(-0.2f, 0.8f).SetEase(Ease.Linear));
        seq.Append(ground_under.transform.DOLocalMoveY(-0.5f, 0.3f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            MainService.instance.gameplayService.playerController.movement.ResetSpeed();
        });
    }
    public void RefreshText()
    {
        int currentBalls = balls.Count;
        currentBalls = Mathf.Clamp(currentBalls, 0, required_balls);
        countText.text = currentBalls + "/" + required_balls;
    }
    private void StartLoseTimer()
    {
        loseSequence = DOTween.Sequence();
        loseSequence.SetId("loseTimer");
        loseSequence.AppendInterval(2.5f);
        loseSequence.AppendCallback(() =>
        {
            MainService.instance.uiService.ChangePanel(UIService.PanelType.Fail);
            failed = true;
        });
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ball")&& !failed)
        {
            if (!balls.Contains(other.gameObject))
            {
                if (!isCompleted)
                {
                    if (loseSequence != null)
                    {
                        print("Restart");
                        DOTween.Restart("loseTimer");
                        //loseSequence.Restart();
                    }
                    else
                    {
                        StartLoseTimer();
                    }
                }

                balls.Add(other.gameObject);
                if (balls.Count >= required_balls && !isCompleted)
                {
                    isCompleted = true;
                    print("Complete");

                    Completed();
                }
                RefreshText();
            }
        }
    }
}
