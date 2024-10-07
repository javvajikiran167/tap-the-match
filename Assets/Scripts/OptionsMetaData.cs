using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TapTheMatch
{
    [Serializable]
    public struct Option
    {
        public string id;
        public string title;
        public Sprite image;
    }

    [CreateAssetMenu(fileName = "OptionsMetaData", menuName = "ScriptableObjects/OptionsMetaData", order = 1)]
    public class OptionsMetaData : ScriptableObject
    {

        public Sprite BG;
        public Sprite optionSelectedHighlightImage;
        public Sprite wrongAnswerImage;
        public Sprite titleBG;
        public List<Option> options;

    }
}

