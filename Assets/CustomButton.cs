using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    public AudioClip clickClip;

    public void RemoveAllAndAddListener(Action onClick)
    {
        base.onClick.RemoveAllListeners();
        AddListener(onClick);
    }

    public void AddListener(Action onClick)
    {
        base.onClick.AddListener(() =>
        {
            PlayClickClip();
            onClick();
        });
    }

    private void PlayClickClip()
    {
        if (clickClip != null)
        {
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        }
    }
}