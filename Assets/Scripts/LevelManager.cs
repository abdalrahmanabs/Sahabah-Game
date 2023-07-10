using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private QuestionTemplate[] questions;
    [SerializeField] private TextMeshProUGUI txtQuestion;
    [SerializeField] private Button[] btnAnswers;
    [SerializeField] TextMeshProUGUI txtCorrectAnswer,txtWrongAnswer;
    private int correctAnswer;

    private void Start()
    {
        LoadQuestions();

        SetQuestion();
    }

    private void SetQuestion()
    {
        QuestionTemplate question = questions[Random.Range(0, questions.Length)];
        correctAnswer = question.correctAnswer;
        txtQuestion.text = FixTextForUI(txtQuestion, question.question, false);

        for (int i = 0; i < question.answers.Length; i++)
        {
            int answerIndex = i;
            btnAnswers[i].GetComponentInChildren<TextMeshProUGUI>().text = FixTextForUI(btnAnswers[i].GetComponentInChildren<TextMeshProUGUI>(), question.answers[i]);
            btnAnswers[i].onClick.AddListener(() => AnswerButtonEvent(answerIndex));
        }
    }

    private void LoadQuestions()
    {
        questions = Resources.LoadAll<QuestionTemplate>("Questions/");
    }

    private void AnswerButtonEvent(int btnId)
    {
        if (btnId == correctAnswer)
        {
            txtWrongAnswer.gameObject.SetActive(false);
            txtCorrectAnswer.gameObject.SetActive(true);
        }
        else
        {
            txtWrongAnswer.gameObject.SetActive(true);
            txtCorrectAnswer.gameObject.SetActive(false);
        }
    }

    private string FixTextForUI(TextMeshProUGUI tmpTextComponent, string fixedText, bool showTashkeel = false, bool useHinduNumbers = false)
    {
        if (string.IsNullOrEmpty(fixedText))
        {
            return null;
        }

        string rtlText = ArabicSupport.Fix(fixedText, showTashkeel, useHinduNumbers);
        rtlText = rtlText.Replace("\r", "");

        string finalText = "";
        string[] rtlParagraph = rtlText.Split('\n');

        tmpTextComponent.text = "";
        for (int lineIndex = 0; lineIndex < rtlParagraph.Length; lineIndex++)
        {
            string[] words = rtlParagraph[lineIndex].Split(' ');
            System.Array.Reverse(words);
            tmpTextComponent.text = string.Join(" ", words);
            Canvas.ForceUpdateCanvases();
            for (int i = 0; i < tmpTextComponent.textInfo.lineCount; i++)
            {
                int startIndex = tmpTextComponent.textInfo.lineInfo[i].firstCharacterIndex;
                int endIndex = (i == tmpTextComponent.textInfo.lineCount - 1) ? tmpTextComponent.text.Length : tmpTextComponent.textInfo.lineInfo[i + 1].firstCharacterIndex;
                int length = endIndex - startIndex;
                string[] lineWords = tmpTextComponent.text.Substring(startIndex, length).Split(' ');
                System.Array.Reverse(lineWords);
                finalText = finalText + string.Join(" ", lineWords).Trim() + "\n";
            }
        }
        tmpTextComponent.text = finalText.TrimEnd('\n');
        return finalText;
    }
}
