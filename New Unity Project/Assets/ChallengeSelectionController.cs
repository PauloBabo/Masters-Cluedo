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

public class ChallengeSelectionController : MonoBehaviour
{
    public GameObject challengeButtonPrefab;
    public RectTransform scrollList;
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    int chapterId = 1;
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
        string query = "SELECT * FROM Challenge WHERE chapterId = " + chapterId ;
        dbcmd.CommandText = query;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            
            GameObject choiceButton = (GameObject)Instantiate(challengeButtonPrefab);
            choiceButton.transform.SetParent(scrollList, false);
            choiceButton.transform.localScale = new Vector3(1, 1, 1);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = reader.GetInt32(1).ToString();

            int challengeId = reader.GetInt32(0);
            string challengeType = reader.GetString(4);
            choiceButton.GetComponent<Button>().onClick.AddListener(() => ChallengeButtonClicked(challengeId, challengeType ));
            
        }
    }

    void ChallengeButtonClicked(int challengeId, string challengeType)
    {
        Debug.Log(challengeId);
        switch (challengeType)
        {
            case "CHOICE":
                SceneManager.LoadScene("choiceScene");
                break;
            case "MCHOICE":
                SceneManager.LoadScene("mchoiceScene");
                break;
            case "FILL":
                SceneManager.LoadScene("fillScene");
                break;
            case "CROSSWORD":
                SceneManager.LoadScene("crosswordScene");
                break;
            case "LETTERS":
                SceneManager.LoadScene("lettersScene");
                break;
            case "PAIR":
                SceneManager.LoadScene("pairScene");
                break;
            case "GROUP":
                SceneManager.LoadScene("groupScene");
                break;
            case "BOSS":
                Console.WriteLine("Case 2");
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
