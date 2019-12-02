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

public class choiceController : MonoBehaviour
{
    string question;
    List<string> options = new List<string>();
    Button[] answerButtons = new Button[4];
    int correctIndex = 3;
    int selectedIndex = -1;
    public TextMeshProUGUI questionText;
    public Button answer1Btn;
    public Button answer2Btn;
    public Button answer3Btn;
    public Button answer4Btn;
    public Button checkBtn;
    public Sprite btnSprite;
    public Sprite checkedBtnSprite;
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        int challengeId = 1;
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
        while (reader.Read())
        {
            if (reader.GetInt32(2) == 1)
            {
                question = reader.GetString(3);
            }
            else if (reader.GetInt32(2) == 2)
            {
                correctIndex = Int32.Parse(reader.GetString(3)) - 1;
            }
            else
            {
                options.Add(reader.GetString(3));
            }
        }
            //TODO get pergunta info from database

        questionText.text = question;
        answerButtons[0] = answer1Btn;
        answerButtons[1] = answer2Btn;
        answerButtons[2] = answer3Btn;
        answerButtons[3] = answer4Btn;

        for(int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
        }
        answerButtons[0].GetComponent<Button>().onClick.AddListener(() => { answerBtnClicked(0); });
        answerButtons[1].GetComponent<Button>().onClick.AddListener(() => { answerBtnClicked(1); });
        answerButtons[2].GetComponent<Button>().onClick.AddListener(() => { answerBtnClicked(2); });
        answerButtons[3].GetComponent<Button>().onClick.AddListener(() => { answerBtnClicked(3); });

        checkBtn.GetComponent<Button>().onClick.AddListener(delegate { checkBtnClicked(); });
    }

    void checkBtnClicked()
    {
        if (selectedIndex < 0)
        {
            Debug.Log("No answer selected!");
        }
        else if (selectedIndex == correctIndex)
        {
            Debug.Log("Correct! You're great!");
        }
        else
        {
            Debug.Log("Incorrect! Better luck next time!");
        }
    }

    void answerBtnClicked(int index)
    {
        //TODO change sprite and change other buttons sprite;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Image>().sprite = btnSprite;
        }
        answerButtons[index].GetComponent<Image>().sprite = checkedBtnSprite;
        selectedIndex = index;
        Debug.Log("Selected: " + index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
