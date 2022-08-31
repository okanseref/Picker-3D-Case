using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Mode
    {
        Normal,
        Endgame,
        None
    }
    public Movement movement;
    public Clicker clicker;
    public List<GameObject> collisionList = new List<GameObject>();//to prevent multiple collisions
    bool levelEnded = false;
    Rigidbody rb;
    void Start()
    {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        clicker = GetComponent<Clicker>();
    }

    void Update()
    {
        
    }
    public void ResetPlayer()
    {
        levelEnded = false;
        transform.rotation = Quaternion.identity;
    }
    public void SwitchMode(Mode mode)
    {
        if (mode == Mode.Normal)
        {
            movement.moveEnabled = true;
            clicker.clickDetection = false;
            clicker.ResetClicker();
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        else if(mode==Mode.Endgame)
        {
            movement.moveEnabled = false;
            clicker.clickDetection = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX;
        }
        else
        {
            
            movement.enabled = false;
            clicker.clickDetection = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (collisionList.Contains(other.gameObject))
        {
            return;
        }
        if (other.tag.Equals("PitStopper"))
        {
            collisionList.Add(other.gameObject);
            movement.currentZspeed = 0;
        }else if (other.tag.Equals("EndGame"))
        {
            collisionList.Add(other.gameObject);
            SwitchMode(Mode.Endgame);
            transform.DOMoveX(0, 0.2f);
        }else if (other.tag.Equals("RampEnd"))
        {
            SwitchMode(Mode.None);
        }else if (other.tag.Equals("MultiplierCube"))
        {
            if (levelEnded)
            {
                return;
            }
            levelEnded = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(transform.DOMove(MainService.instance.levelService.nextLevel.transform.position, 0.5f));
            seq.AppendCallback(() =>
            {
                MainService.instance.uiService.ChangePanel(UIService.PanelType.EndGame);
                MainService.instance.gameplayService.NextLevel();
            });
        }
    }
}
