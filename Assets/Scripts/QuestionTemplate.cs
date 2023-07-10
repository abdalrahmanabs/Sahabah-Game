using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "Data", menuName = "Questions/Question", order = 1)]

public class QuestionTemplate: ScriptableObject
{

    
    
    [Range(1,3)] public int diff = 1;



    public string question;

    [TextArea(5, 7)]
    public string fixedQuestion;
    [Space(7)]

    [TextArea(2, 3)] 
    public string[] answers = new string[4] { "", "", "", "" };

    public int correctAnswer;

    public bool hasMoreThanOnePossibleAnswer;

    public string[] possibleAnswers;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Debug.Log("SDJF:SDKFJ");
        fixedQuestion = ArabicSupport.Fix(question);
    }


#endif
    
}
