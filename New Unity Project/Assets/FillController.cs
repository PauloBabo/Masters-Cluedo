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

public class FillController : MonoBehaviour
{
    public GameObject fillButtonPrefab;
    public TextMeshProUGUI questionText;
    public RectTransform ParentPanel;
    public Button checkButton;
    string question;
    string formatString;
    string[] questionTexts;
    string[] spaceTexts;
    List<string> options = new List<string>();
    Button[] choiceButtons;
    List<int> correctIndexes = new List<int>();
    List<int> selectedIndexes = new List<int>();
    List<bool> selectedButtons = new List<bool>();
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    int challengeId = 3;
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
        while (reader.Read())
        {
            if (reader.GetInt32(2) == 1)
            {
                question = reader.GetString(3);
            }
            else if (reader.GetInt32(2) == 2)
            {
                string[] indexesString = reader.GetString(3).Split('/');
                for (int i = 0; i < indexesString.Length; i++)
                {
                    correctIndexes.Add(Int32.Parse(indexesString[i]) - 1);
                }
            }
            else
            {
                options.Add(reader.GetString(3));
                selectedButtons.Add(false);
                selectedIndexes.Add(-1);
            }
        }
        string needle = "(_)";
        int nSpaces = (question.Length - question.Replace(needle, "").Length) / needle.Length;
        questionTexts = question.Split(new string[] { "(_)" }, System.StringSplitOptions.None);
        spaceTexts = new string[nSpaces];
        for (int i = 0; i < spaceTexts.Length; i++)
        {
            spaceTexts[i] = "____________";
        }

        formatString = CreateFormatString(question);
        UpdateQuestionText();

        choiceButtons = new Button[options.Count];
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            GameObject choiceButton = (GameObject)Instantiate(fillButtonPrefab);
            choiceButton.transform.SetParent(ParentPanel, false);
            choiceButton.transform.localScale = new Vector3(1, 1, 1);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            Button tempButton = choiceButton.GetComponentInChildren<Button>();
            int index = i;
            tempButton.onClick.AddListener(() => OptionButtonClicked(index));
            choiceButtons[i] = tempButton;
        }
        checkButton.GetComponent<Button>().onClick.AddListener(delegate { CheckButtonClicked(); });
    }

    void OptionButtonClicked(int index)
    {
        if (!selectedButtons[index])
        {
            bool updated = false;
            for (int i = 0; i < selectedIndexes.Count; i++)
            {
                if (selectedIndexes[i] == -1)
                {
                    selectedIndexes[i] = index;
                    spaceTexts[i] = "<u>" + options[index] + "</u>";
                    UpdateQuestionText();
                    updated = true;
                    break;
                }
            }
            if(updated)
            {
                selectedButtons[index] = true;
                choiceButtons[index].GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            for (int i = 0; i < selectedIndexes.Count; i++)
            {
                if (selectedIndexes[i] == index)
                {
                    selectedIndexes[i] = -1;
                    spaceTexts[i] = "____________";
                    UpdateQuestionText();
                    break;
                }
            }
            selectedButtons[index] = false;
            choiceButtons[index].GetComponent<Image>().color = Color.white;
        }
        
    }

    string CreateFormatString(string text)
    {
        string formatString = "";
        string[] words;

        words = text.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Equals("(_)"))
            {
                formatString += " SPACE ";
            }
            else
            {
                formatString += "S";
            }
        }
        if (formatString[0].Equals(' '))
        {
            formatString = formatString.Remove(0, 1);
        }
        if (formatString[formatString.Length - 1].Equals(' '))
        {
            formatString = formatString.Remove(formatString.Length - 1);
        }

        Debug.Log(formatString);
        return formatString;
    }

    void UpdateQuestionText()
    {
        string[] formatWords;
        string currentTextString = "";
        formatWords = formatString.Split(' ');
        int currentSpaceText = 0;
        int currentQuestionText = 0;
        for (int i = 0; i < formatWords.Length; i++)
        {
            if (formatWords[i].Equals("SPACE"))
            {
                currentTextString += " " + spaceTexts[currentSpaceText] + " ";
                currentSpaceText++;
            }
            else
            {
                currentTextString += questionTexts[currentQuestionText]; 
                currentQuestionText++;
            }
        }
        questionText.text = currentTextString;
    }

    void CheckButtonClicked()
    {
        for (int i = 0; i < correctIndexes.Count; i++)
        {
            if (selectedIndexes[i] != correctIndexes[i])
            {
                Debug.Log("Incorrect answer!");
                return;
            }
        }
        Debug.Log("Correct answer!");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
