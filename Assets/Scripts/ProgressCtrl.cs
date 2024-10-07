using UnityEngine;
using UnityEngine.UI;

public class ProgressCtrl : MonoBehaviour
{

    public Image image;

    public void SetProgress(float value)
    {
        image.fillAmount = value;
    }
}
