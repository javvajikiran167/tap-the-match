using System.Collections.Generic;
using System.Linq;
using Custom.Utils;
using UnityEngine;

namespace TapTheMatch.Question
{
    public class DisplayQuestion : MonoBehaviour
    {
        private const int OPTIONS_COUNT = 4;
        public TitleCtrl titleCtrl;
        public OptionCtrl[] optionCtrls = new OptionCtrl[OPTIONS_COUNT];
        public OptionsMetaData colorsMetaData;

        private void Start()
        {
            DisplayQuestionData(colorsMetaData);
        }

        public void DisplayQuestionData(OptionsMetaData questionsData)
        {
            var optionsData = questionsData.options.GetRandom(OPTIONS_COUNT);
            var optionsTitles = optionsData.Select(x => x.title).ToList();
            optionsTitles.Shuffle();

            GetTitleData(optionsData, optionsTitles, out string questionTitle, out int answerIndex);
            titleCtrl.SetOption(questionTitle, questionsData.titleBG);

            Debug.Log($"Answer Index: {answerIndex}");
            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                int currentIndex = i;
                Debug.Log($"Option {currentIndex} : {optionsTitles[currentIndex]}");

                optionCtrls[currentIndex].SetOption(optionsTitles[currentIndex], optionsData[currentIndex].image, () =>
                {
                    OnOptionSelection(answerIndex, currentIndex);
                });
            }

        }

        private void OnOptionSelection(int answerIndex, int selectedIndex)
        {
            Debug.Log($"Option {selectedIndex} Clicked");

            if (selectedIndex == answerIndex)
            {
                Debug.Log("Correct Answer");
            }
            else
            {
                Debug.Log("Wrong Answer");
            }

            CoroutineUtils.instance.WaitUntillGivenTime(1f, () =>
            {
                DisplayQuestionData(colorsMetaData);
            });
        }

        private void GetTitleData(List<Option> optionsData, List<string> optionsTitles, out string questionTitle, out int answerIndex)
        {
            int randomIndex = Random.Range(0, optionsData.Count);
            bool isColorQuestion = Random.Range(0, 2) == 0;
            questionTitle = isColorQuestion ? $"Color\n{optionsData[randomIndex].title}" : $"Text\n{optionsData[randomIndex].title}";
            answerIndex = isColorQuestion ? randomIndex : optionsTitles.IndexOf(optionsData[randomIndex].title);
        }
    }
}