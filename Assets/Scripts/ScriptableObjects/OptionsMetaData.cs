using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "OptionsMetaData", menuName = "ScriptableObjects/OptionsMetaData", order = 1)]
public class OptionsMetaData : ScriptableObject
{


    [Serializable]
    public struct Option
    {
        public string title;
        public Sprite image;
    }

    public Sprite BG;
    public Sprite selectedImage;
    public Sprite wrongAnswerImage;

    public List<Option> options;

}
