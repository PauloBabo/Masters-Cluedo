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

public class mchoiceController : MonoBehaviour
{
    public GameObject choiceButtonPrefab;
    public RectTransform ParentPanel;
    public TextMeshProUGUI questionText;
    public Sprite selectedChoiceBtnSprite;
    public Sprite choiceBtnSprite;
    public Button checkButton;
    private string question;
    private List<string> options = new List<string>();
    private List<bool> selected = new List<bool>();
    private List<Button> choiceButtons = new List<Button>();
    private List<int> correctIndexes = new List<int>();
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    int challengeId = 2;
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
                    correctIndexes.Add(Int32.Parse(indexesString[i])-1);
                }
            }
            else
            {
                options.Add(reader.GetString(3));
                selected.Add(false);
            }
        }
        
        questionText.text = question;

        checkButton.onClick.AddListener(() => CheckButtonClicked());
        for (int i = 0; i < options.Count; i++)
        {
            GameObject choiceButton = (GameObject)Instantiate(choiceButtonPrefab);
            choiceButton.transform.SetParent(ParentPanel, false);
            choiceButton.transform.localScale = new Vector3(1, 1, 1);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            Button tempButton = choiceButton.GetComponent<Button>();
            int index = i;
            tempButton.onClick.AddListener(() => ChoiceButtonClicked(index));
            choiceButtons.Add(tempButton);
        }
    }

    void ChoiceButtonClicked(int index)
    {
        if(!selected[index])
        {
            selected[index] = true;
            choiceButtons[index].GetComponent<Image>().sprite = selectedChoiceBtnSprite;
        }
        else
        {
            selected[index] = false;
            choiceButtons[index].GetComponent<Image>().sprite = choiceBtnSprite;
        }
    }

    void CheckButtonClicked()
    {
        List<int> selectedIndexes = new List<int>();
        for (int i = 0; i < selected.Count; i++)
        {
            if (selected[i])
            {
                selectedIndexes.Add(i);
            }
        }
        if (selectedIndexes.Count != correctIndexes.Count)
        {
            Debug.Log("Resposta errada!");
            return;
        }
        else
        {
            for (int i = 0; i < correctIndexes.Count; i++)
            {
                if (selectedIndexes[i] != correctIndexes[i])
                {
                    Debug.Log("Resposta errada!");
                    return;
                }
            }
            Debug.Log("Resposta certa!");

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
