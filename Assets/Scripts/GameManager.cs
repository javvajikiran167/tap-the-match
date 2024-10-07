using TapTheMatch.Question;
using UnityEngine;

namespace TapTheMatch
{
    public class GameManager : MonoBehaviour
    {
        public TutorialCtrl tutorialCtrl;
        public QuestionCtrl questionCtrl;
        public ScoreCtrl scoreCtrl;
        public GameOverCtrl gameOverCtrl;
        public ProgressCtrl progressCtrl;
        public AudioCtrl audioCtrl;
        public OptionsMetaData optionsMetaData;
        public GameObject gamePlayUI;

        private TimerCtrl timerCtrl;

        private const int QUESTION_TIME = 10;

        private void Start()
        {
            if (PlayerPrefs.GetInt("FirstTime1", 0) == 0)
            {
                Debug.Log("First Time");
                PlayerPrefs.SetInt("FirstTime", 1);
                tutorialCtrl.OpenTutorial(Init);
            }
            else
            {
                Init();
            }
        }

        private void Init()
        {
            Debug.Log("Init");
            gamePlayUI.SetActive(true);
            questionCtrl.Init(optionsMetaData, OnAnsweringAQuestion, optionsMetaData.optionSelectedHighlightImage);
            scoreCtrl.Init();
            gameOverCtrl.Init(optionsMetaData.wrongAnswerImage);
            timerCtrl = new TimerCtrl(OnTimeUp, progressCtrl.SetProgress);

            DisplayNextQuestion();
        }

        private void DisplayNextQuestion()
        {
            questionCtrl.NextQuestion();
            progressCtrl.SetProgress(0);
            timerCtrl.StartTimer(QUESTION_TIME);
        }

        private void OnAnsweringAQuestion(bool isCorrect)
        {
            timerCtrl.StopTimer();

            if (isCorrect)
            {
                HandleCorrectAnswer();
            }
            else
            {
                HandleWrongAnswer();
            }
        }

        private void HandleCorrectAnswer()
        {
            audioCtrl.PlaySuccessClip();
            scoreCtrl.IncreaseScore(timerCtrl.GetNormalizedRemainingTime());
            DisplayNextQuestion();
        }

        private void HandleWrongAnswer()
        {
            audioCtrl.PlayFailClip();
            gameOverCtrl.ShowGameOver(scoreCtrl.GetCurrentScore(), OnRestart);
        }

        private void OnTimeUp()
        {
            gameOverCtrl.ShowGameOver(scoreCtrl.GetCurrentScore(), OnRestart);
        }

        private void OnRestart()
        {
            Init();
        }
    }
}