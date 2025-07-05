using System;
using Custom.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapTheMatch
{
    public class GameOverCtrl : MonoBehaviour
    {
        public GameObject wrongAnswerGo;
        public GameObject playAgainUIGo;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI highScoreText;
        public CustomButton restartBtn;
        public CustomButton exitBtn;

        internal void Init(Sprite wrongAnswerSprite)
        {
            wrongAnswerGo.GetComponent<Image>().sprite = wrongAnswerSprite;
            wrongAnswerGo.SetActive(false);
            playAgainUIGo.SetActive(false);
        }

        internal void ShowGameOver(int score, Action restartCallback)
        {
            wrongAnswerGo.SetActive(true);
            CoroutineUtils.Instance.WaitUntilGivenTime(1, () =>
            {
                wrongAnswerGo.SetActive(false);
                DisplayPlayAgainScreen(score, restartCallback);
            });
        }

        private void DisplayPlayAgainScreen(int score, Action restartCallback)
        {
            playAgainUIGo.SetActive(true);

            int highScore = PlayerPrefs.GetInt("HighScore");
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
            }

            highScoreText.text = $"High Score: {highScore}";
            scoreText.text = $"Score: {score}";

            restartBtn.RemoveAllAndAddListener(restartCallback);
            exitBtn.RemoveAllAndAddListener(Application.Quit);
        }
    }
}