using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundService : MonoBehaviour,LevelPreparer
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    public int slotCount = 10;
    void Start()
    {

    }
    public void PlaySound(int index,float volume)
    {
        if (MainService.instance.dataService.soundOn == true)
        {
            if (slotCount > 0)
            {
                slotCount--;
                audioSource.PlayOneShot(sounds[index], volume);
                StartCoroutine(ReleaseSlot());
            }

        }
    }
    IEnumerator ReleaseSlot()
    {
        yield return new WaitForSeconds(0.3f);
        slotCount++;
    }
    public void PlaySoundDelayed(int index,float delay)
    {
        if (MainService.instance.dataService.soundOn == true)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay).OnComplete(() => audioSource.PlayOneShot(sounds[index], 1f));
            sequence.Play();
        }
    }
    public void Vibrate(long ms)
    {
        if (!Application.isEditor &&MainService.instance.dataService.vibrationOn)
        {
            Vibration1 vb = new Vibration1();

            vb.Vibrate(ms);
        }
    }
    public void PrepareLevel()
    {
        audioSource.Stop();
    }
}
