using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class GroupController : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public GameObject startButton;
    public GameObject timer;
    public TextMeshProUGUI questionText;
    List<GroupQuestion> groupQuestions = new List<GroupQuestion>();
    List<Button> buttons = new List<Button>();
    int currentQuestionIndex = 0;
    int challengeId = 7;
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        string DatabaseName = "Cluedo_DB.s3db";
        string filepath = Application.dataPath + "/Plugins/" + DatabaseName;
        conn = "URI=file:" + filepath;
        Debug.Log("Stablishing connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        string query = "SELECT * FROM Arguments WHERE challengId = " + challengeId;
        dbcmd.CommandText = query;
        IDataReader reader = dbcmd.ExecuteReader();
        string options = "";
        string leftQuestions = "";
        string rightQuestions = "";
        while (reader.Read())
        {
            if (reader.GetInt32(2) == 1)
            {
                options = reader.GetString(3);
            }
            else if (reader.GetInt32(2) == 2)
            {
                leftQuestions = reader.GetString(3);
            }
            else if (reader.GetInt32(2) == 3)
            {
                rightQuestions = reader.GetString(3);
            }
        }

        buttons.Add(leftButton);
        buttons.Add(rightButton);

       
 

        string[] optionsSeparated = options.Split(new string[] { " // " }, System.StringSplitOptions.None);
        string[] leftQuestionsSeparated = leftQuestions.Split(new string[] { " // " }, System.StringSplitOptions.None);
        string[] rightQuestionsSeparated = rightQuestions.Split(new string[] { " // " }, System.StringSplitOptions.None);

        for (int i = 0; i < leftQuestionsSeparated.Length; i++)
        {
            groupQuestions.Add(new GroupQuestion(leftQuestionsSeparated[i], 0));
        }
        for (int i = 0; i < rightQuestionsSeparated.Length; i++)
        {
            groupQuestions.Add(new GroupQuestion(rightQuestionsSeparated[i], 1));
        }

        groupQuestions.Shuffle();

        questionText.text = groupQuestions[currentQuestionIndex].questionString;
        leftButton.GetComponentInChildren<TextMeshProUGUI>().text = optionsSeparated[0];
        rightButton.GetComponentInChildren<TextMeshProUGUI>().text = optionsSeparated[1];

        startButton.GetComponent<Button>().onClick.AddListener(() => StartButtonClicked());
        leftButton.GetComponent<Button>().onClick.AddListener(() => OptionButtonClicked(0));
        rightButton.GetComponent<Button>().onClick.AddListener(() => OptionButtonClicked(1));

        leftButton.enabled = false;
        rightButton.enabled = false;
    }

    void StartButtonClicked()
    {
        timer.GetComponent<Timer>().counting = true;
        Destroy(startButton);
        leftButton.enabled = true;
        rightButton.enabled = true;
    }

    void OptionButtonClicked(int sideIndex)
    {
        buttons[groupQuestions[currentQuestionIndex].solutionIndex].GetComponent<Image>().color = Color.green;
        if (sideIndex == groupQuestions[currentQuestionIndex].solutionIndex)
        {
            groupQuestions[currentQuestionIndex].correct = true;
        }
        else 
        {
            groupQuestions[currentQuestionIndex].correct = false;
        }
        timer.GetComponent<Timer>().counting = false;
        leftButton.enabled = false;
        rightButton.enabled = false;
        StartCoroutine(NextQuestionCourotine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuestionTimeOut()
    {
        buttons[groupQuestions[currentQuestionIndex].solutionIndex].GetComponent<Image>().color = Color.green;
        groupQuestions[currentQuestionIndex].correct = false;
        
        timer.GetComponent<Timer>().counting = false;
        leftButton.enabled = false;
        rightButton.enabled = false;
        StartCoroutine(NextQuestionCourotine());
    }

    private IEnumerator NextQuestionCourotine()
    {
        yield return new WaitForSeconds(2);
        currentQuestionIndex++;
        if (currentQuestionIndex < groupQuestions.Count)
        {
            timer.GetComponent<Timer>().Reset();
            timer.GetComponent<Timer>().counting = true;
            buttons[0].GetComponent<Image>().color = Color.white;
            buttons[1].GetComponent<Image>().color = Color.white;
            questionText.text = groupQuestions[currentQuestionIndex].questionString;
            leftButton.enabled = true;
            rightButton.enabled = true;
        }
        else
        {   
            //TODO compôr
            for (int i = 0; i < groupQuestions.Count; i++)
            {
                if (groupQuestions[i].correct)
                {
                    Debug.Log("Correct: " + groupQuestions[i].questionString);
                }
                else
                {
                    Debug.Log("Incorrect: " + groupQuestions[i].questionString);
                }
            }
        }

    }
}

public class GroupQuestion
{
    public string questionString;
    public int solutionIndex;
    public bool correct;
    public GroupQuestion(string questionString, int solutionIndex)
    {
        this.questionString = questionString;
        this.solutionIndex = solutionIndex;
    }

}