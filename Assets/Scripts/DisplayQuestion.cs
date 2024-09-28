using System.Collections.Generic;
using System.Linq;
using Custom.Utils;
using UnityEngine;

namespace TapTheMatch.Question
{
    public class DisplayQuestion : MonoBehaviour
    {

        const int OPTIONS_COUNT = 4;

        public TitleCtrl titleCtrl;
        public OptionCtrl[] optionCtrls = new OptionCtrl[OPTIONS_COUNT];
        public OptionsMetaData colorsMetaData;


        void Start()
        {
            DisplayQuestionData(colorsMetaData);
        }

        public void DisplayQuestionData(OptionsMetaData questionsData)
        {
            List<Option> optionsData = questionsData.options.GetRandom(OPTIONS_COUNT);
            List<string> optionsTitles = optionsData.Select(x => x.title).ToList();
            optionsTitles.Shuffle();

            int randomIndex = Random.Range(0, optionsData.Count);
            int answerIndex = 0;

            string questionTitle;
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                questionTitle = "Color\n" + optionsData[randomIndex].title;
                answerIndex = randomIndex;
            }
            else
            {
                questionTitle = "Text\n" + optionsData[randomIndex].title;
                answerIndex = optionsTitles.FindIndex(x => x == optionsData[randomIndex].title);
            }

            Debug.Log("Answer Index: " + answerIndex);

            for (int i = 0; i < OPTIONS_COUNT; i++)
            {
                Debug.Log("Option " + i + " : " + optionsTitles[i]);

                optionCtrls[i].SetOption(optionsTitles[i], optionsData[i].image, () =>
                {
                    int clickedOption = i;
                    Debug.Log("Option " + clickedOption + " Clicked");

                    if (clickedOption == answerIndex)
                    {
                        Debug.Log("Correct Answer");
                    }
                    else
                    {
                        Debug.Log("Wrong Answer");
                    }

                    CoroutineUtils.instance.WaitUntillGivenTime(1f, () =>
                    {
                        DisplayQuestionData(questionsData);
                    });
                });
            }

            titleCtrl.SetOption(questionTitle, questionsData.titleBG);
        }
    }
}