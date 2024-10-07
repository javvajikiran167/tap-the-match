using System;
using UnityEngine;
using UnityEngine.UI;

namespace TapTheMatch.Question
{

    [RequireComponent(typeof(Button))]
    public class OptionCtrl : TitleCtrl
    {
        private Button button;
        public Image selectedHighlightImage;

        public void Init(Sprite selectedHighlightSprite)
        {
            selectedHighlightImage.sprite = selectedHighlightSprite;
            selectedHighlightImage.gameObject.SetActive(false);
        }

        public void SetOption(string title, Sprite image, Action onClick)
        {
            selectedHighlightImage.gameObject.SetActive(false);
            SetButtonClick(onClick);
            base.SetOption(title, image);
        }

        private void SetButtonClick(Action onClick)
        {
            if (button == null)
            {
                button = this.gameObject.GetComponent<Button>();
            }

            if (onClick == null)
            {
                return;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                selectedHighlightImage.gameObject.SetActive(true);
                onClick();
            });
        }
    }
}