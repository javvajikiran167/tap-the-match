using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapTheMatch
{
    public struct Question
    {
        public string title;
        public Sprite image;
    }

    [RequireComponent(typeof(Button))]
    public class QuestionItemCtrl : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI title;
        private Button button;


        public void SetQuestionItem(Question question, Action onClick)
        {
            SetButtonClick(onClick);
            SetTitle(question.title);
            SetImage(question.image);
        }

        private void SetButtonClick(Action onClick)
        {
            if (button == null)
            {
                button = this.gameObject.GetComponent<Button>();
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick());
        }

        private void SetTitle(string title)
        {
            if (this.title == null)
            {
                Debug.Log("Question text go is not set");
                return;
            }

            if (string.IsNullOrEmpty(title))
            {
                this.title.gameObject.SetActive(false);
                return;
            }

            this.title.text = title;
            this.title.gameObject.SetActive(true);

        }

        private void SetImage(Sprite image)
        {
            if (this.image == null)
            {
                Debug.Log("Question image go is not set");
                return;
            }

            if (image == null)
            {
                this.image.gameObject.SetActive(false);
                return;
            }

            this.image.sprite = image;
            this.image.gameObject.SetActive(true);
        }
    }
}