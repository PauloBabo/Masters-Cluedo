using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//https://github.com/walidabazo/SQLiteUnity3d_Android/blob/master/Unity3d_Android_Sqlite

public class loadingController : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;

    string DatabaseName = "Cluedo_DB.s3db";
    void Start()
    {
        string filepath = Application.dataPath + "/Plugins/" + DatabaseName;
        //todo test this
        /*
        if (!File.Exists(filepath))
        {
            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                             Application.dataPath + "!/assets/Cluedo_DB");
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/Cluedo_DB.s3db");
            while (!loadDB.isDone) { }
            File.WriteAllBytes(filepath, loadDB.bytes);
        }*/

        conn = "URI=file:" + filepath;
        Debug.Log("Stablishing connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        string databaseLoaded = "false";
        IDbCommand dbcmd = dbconn.CreateCommand();
        string query = "SELECT databaseLoaded FROM PlayerStats";// table name
        dbcmd.CommandText = query;
        IDataReader reader = dbcmd.ExecuteReader();
        
        while (reader.Read())
        {
            databaseLoaded = reader.GetString(0);
            Debug.Log("Loaded:" + databaseLoaded);
        }
        if (databaseLoaded.Equals("false"))
        {
            int chapterID = 1;
            int challengeID = 1;
            string chapterFileName = "CHAPTER_1.csv";
            using (StreamReader sr = new StreamReader("Assets/Resources/Chapters/" + chapterFileName))
            {
                int currentLine = 1;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.Log(line);
                    string[] challengeComponents = line.Split(',');
                    if (currentLine == 1)
                    {
                        query = "INSERT INTO Chapter (name,color1,color2,color3,locked) VALUES ('" + challengeComponents[0] + "','" + challengeComponents[1] + "','" + challengeComponents[2] + "','" + challengeComponents[3] + "','false')";
                        dbcmd = dbconn.CreateCommand();
                        dbcmd.CommandText = query;
                        dbcmd.ExecuteScalar();
                    }
                    else if (currentLine == 2)
                    {

                    }
                    else
                    {
                        if (challengeComponents[1] != "")
                        {
                            if (challengeComponents[1] != "DIALOG" && challengeComponents[1] != "BOSS") //todo
                            {
                                query = "INSERT INTO Challenge (number,type,chapterId,locked) VALUES (" + challengeComponents[0] + ",'" + challengeComponents[1] + "'," + chapterID + ",'false')";
                                dbcmd = dbconn.CreateCommand();
                                dbcmd.CommandText = query;
                                dbcmd.ExecuteScalar();

                                for (int i = 2; i < challengeComponents.Length; i++)
                                {
                                    if (challengeComponents[i] != "")
                                    {
                                        int number = i - 1;
                                        query = "INSERT INTO Arguments (number,challengId,content) VALUES (" + number + "," + challengeID + ",'" + challengeComponents[i].Replace('"', " ".ToCharArray()[0]).Trim() + "')";
                                        dbcmd = dbconn.CreateCommand();
                                        dbcmd.CommandText = query;
                                        dbcmd.ExecuteScalar();
                                    }
                                }
                                challengeID++;
                            }
                        }
                    }
                    currentLine++;
                }
            }
        }
        query = "UPDATE PlayerStats set databaseLoaded = 'true'";
        dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.ExecuteScalar();
        dbconn.Close();
    }

    void Update()
    {
        SceneManager.LoadScene("mainMenuScene");
    }
}