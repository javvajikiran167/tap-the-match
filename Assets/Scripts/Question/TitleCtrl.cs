using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapTheMatch.Question
{
    public class TitleCtrl : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI title;

        public void SetOption(string title, Sprite image)
        {
            SetTitle(title);
            SetImage(image);
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