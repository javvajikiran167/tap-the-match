using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TapTheMatch.Question
{
    public class QuestionCtrl : MonoBehaviour
    {
        private const int OPTIONS_COUNT = 4;
        public TitleCtrl titleCtrl;
        public OptionCtrl[] optionCtrls = new OptionCtrl[OPTIONS_COUNT];

        private OptionsMetaData questionsMetaData;
        private Action<bool> onAnsweringAQuestion;

        internal void Init(OptionsMetaData optionsMetaData, Action<bool> answerCallback, Sprite optionSelectedHighlightSprite)
        {
            this.questionsMetaData = optionsMetaData;
            this.onAnsweringAQuestion = answerCallback;

            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                optionCtrls[i].Init(optionSelectedHighlightSprite);
            }
        }


        internal void NextQuestion()
        {
            SetQuestion();
        }

        public void SetQuestion()
        {
            var optionsData = questionsMetaData.options.GetRandom(OPTIONS_COUNT);
            var optionsTitles = optionsData.Select(x => x.title).ToList();
            optionsTitles.Shuffle();

            GetTitleData(optionsData, optionsTitles, out string questionTitle, out int answerIndex);
            titleCtrl.SetOption(questionTitle, questionsMetaData.titleBG);

            Debug.Log($"Answer Index: {answerIndex}");
            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                int currentIndex = i;
                Debug.Log($"Option {currentIndex} : {optionsTitles[currentIndex]}");

                optionCtrls[currentIndex].SetOption(optionsTitles[currentIndex], optionsData[currentIndex].image, () =>
                {
                    onAnsweringAQuestion(answerIndex == currentIndex);
                });
            }
        }

        private void GetTitleData(List<Option> optionsData, List<string> optionsTitles, out string questionTitle, out int answerIndex)
        {
            int randomIndex = UnityEngine.Random.Range(0, optionsData.Count);
            bool isColorQuestion = UnityEngine.Random.Range(0, 2) == 0;
            questionTitle = isColorQuestion ? $"Color\n{optionsData[randomIndex].title}" : $"Text\n{optionsData[randomIndex].title}";
            answerIndex = isColorQuestion ? randomIndex : optionsTitles.IndexOf(optionsData[randomIndex].title);
        }
    }
}