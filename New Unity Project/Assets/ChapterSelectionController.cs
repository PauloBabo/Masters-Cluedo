using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using TMPro;
public class ChapterSelectionController : MonoBehaviour
{
    public GameObject chapterButtonPrefab;
    public RectTransform scrollList;
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
        string query = "SELECT * FROM Chapter";// table name
        dbcmd.CommandText = query;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            GameObject choiceButton = (GameObject)Instantiate(chapterButtonPrefab);
            choiceButton.transform.SetParent(scrollList, false);
            choiceButton.transform.localScale = new Vector3(1, 1, 1);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = reader.GetString(1);
            Color buttonColor;
            Color textColor;
            int chapterId = reader.GetInt32(0);
            ColorUtility.TryParseHtmlString("#"+reader.GetString(2), out buttonColor);
            ColorUtility.TryParseHtmlString("#"+reader.GetString(4), out textColor);
            choiceButton.GetComponent<Image>().color = buttonColor;
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
            choiceButton.GetComponent<Button>().onClick.AddListener(() => ChapterButtonClicked(chapterId));
        }
    }

    void ChapterButtonClicked(int chapterId)
    {
        SceneManager.LoadScene("challengeSelectionScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
