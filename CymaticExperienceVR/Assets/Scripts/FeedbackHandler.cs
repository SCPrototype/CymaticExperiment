﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackHandler : MonoBehaviour
{
    public bool StartOnAwake = true;
    public string[] FeedbackQuestions;
    public string FinishedText;
    private int _currentQuestion = -1;

    [Space(10)]
    public TextMesh QuestionText;
    public VotingObject MyVotingObject;
    public GameObject AnswerObjects;

    private System.IO.StreamWriter _streamWriter;
    private string _path = "Assets/Resources/Answers/Answers.txt";

    // Start is called before the first frame update
    void Start()
    {
        System.IO.FileStream test = new System.IO.FileStream(_path, System.IO.FileMode.Append);
        _streamWriter = new System.IO.StreamWriter(test);

        if (StartOnAwake)
        {
            AskNextQuestion();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AskNextQuestion();
        }
    }


    private void OnDestroy()
    {
        _streamWriter.WriteLine("__________");
        _streamWriter.Close();
    }

    private void OnApplicationQuit()
    {
        _streamWriter.WriteLine("__________");
        _streamWriter.Close();
    }

    public void StoreAnswer(int pIndex)
    {
        _streamWriter.WriteLine(pIndex + "\t" + FeedbackQuestions[_currentQuestion]);
        MyVotingObject.ForceRespawn();
        AskNextQuestion();
    }

    private void AskNextQuestion()
    {
        if (_currentQuestion + 1 < FeedbackQuestions.Length)
        {
            _currentQuestion++;
            QuestionText.text = FeedbackQuestions[_currentQuestion];
        }
        else
        {
            EndQuestioning();
        }
    }

    private void AskQuestion(int pIndex)
    {
        if (pIndex >= 0)
        {
            _currentQuestion = pIndex -1;
            AskNextQuestion();
        }
    }

    private void EndQuestioning()
    {
        QuestionText.text = FinishedText;
        AnswerObjects.SetActive(false);
    }
}
