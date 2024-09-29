using System;
using UnityEngine;
using UnityEngine.UI;

namespace TapTheMatch.Question
{

    [RequireComponent(typeof(Button))]
    public class OptionCtrl : TitleCtrl
    {
        private Button button;

        public void SetOption(string title, Sprite image, Action onClick)
        {
            SetButtonClick(onClick);
            base.SetOption(title, image);
        }

        private void SetButtonClick(Action onClick)
        {
            if (button == null)
            {
                button = this.gameObject.GetComponent<Button>();
            }

            button.onClick.RemoveAllListeners();

            if (onClick == null)
            {
                return;
            }
            button.onClick.AddListener(() => onClick());
        }
    }
}