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
    public InsideCounter insideCounter;
    public GameObject rotatingArms;
    public List<GameObject> collisionList = new List<GameObject>();//to prevent multiple collisions
    bool levelEnded = false;
    Rigidbody rb;
     void Awake()
    {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        clicker = GetComponent<Clicker>();
    }
    void Start()
    {
        rotatingArms.SetActive(false);
    }

    public void StopSpeed()
    {
        movement.moveEnabled = false;
    }
    public void ResetPlayer()
    {
        levelEnded = false;
        movement.ResetSpeed();
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
            
            movement.moveEnabled = false;
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
            rotatingArms.SetActive(false);
            movement.currentZspeed = 0;
            foreach (GameObject item in insideCounter.list)
            {
                item.GetComponent<Rigidbody>().AddForce(2*(Vector3.forward + Vector3.up),ForceMode.VelocityChange);
            }
        }else if (other.tag.Equals("EndGame"))
        {
            collisionList.Add(other.gameObject);
            SwitchMode(Mode.Endgame);
            transform.DOMoveX(0, 0.2f);
            MainService.instance.uiService.ToggleFillPanel(true);
        }else if (other.tag.Equals("RampEnd"))
        {
            SwitchMode(Mode.None);
            MainService.instance.uiService.ToggleFillPanel(false);
        }else if (other.tag.Equals("PowerUp"))
        {
            other.gameObject.SetActive(false);
            rotatingArms.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                rotatingArms.SetActive(false);
            });
        }
        else if (other.tag.Equals("MultiplierCube"))
        {
            if (levelEnded)
            {
                return;
            }
            levelEnded = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Vector3 movePosition = MainService.instance.levelService.nextLevel.transform.position;
            movePosition.y += 0.2f;
            transform.rotation = Quaternion.identity;
            MainService.instance.uiService.ChangePanel(UIService.PanelType.EndGame);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(transform.DOMove(movePosition, 0.5f));
            seq.AppendInterval(0.5f);
            seq.AppendCallback(() =>
            {
                MainService.instance.gameplayService.NextLevel();
            });
        }
    }
}
