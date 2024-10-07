using TMPro;
using UnityEngine;

namespace TapTheMatch
{
    public class ScoreCtrl : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        private int currentScore;

        private const int MAX_SCORE_PER_QUESTION = 100;
        private const int MIN_SCORE_PER_QUESTION = 10;

        internal void Init()
        {
            ResetScore();
        }

        internal void IncreaseScore(float normalizedTimeRemaining)
        {
            int increment = (int)(MIN_SCORE_PER_QUESTION + (MAX_SCORE_PER_QUESTION - MIN_SCORE_PER_QUESTION) * normalizedTimeRemaining);
            currentScore += increment;
            DisplayScore();
        }

        internal void ResetScore()
        {
            currentScore = 0;
            DisplayScore();
        }

        private void DisplayScore()
        {
            scoreText.text = $"Score: {currentScore}";
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }
    }
}