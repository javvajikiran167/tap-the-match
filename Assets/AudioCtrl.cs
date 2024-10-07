using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    public AudioClip btnClickClip;
    public AudioClip successClip;
    public AudioClip failClip;

    public void PlayBtnClickClip()
    {
        PlayClip(btnClickClip);
    }

    public void PlaySuccessClip()
    {
        PlayClip(successClip);
    }

    public void PlayFailClip()
    {
        PlayClip(failClip);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }



}
