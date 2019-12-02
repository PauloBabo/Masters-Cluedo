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

public class PairController : MonoBehaviour
{
    public GameObject pairButtonPrefab;
    public GameObject lineRendererPrefab;
    public RectTransform ParentPanel;
    public RectTransform scrollListViewport;
    public Button checkButton;
    List<string> correctPairStrings;
    List<Pair> correctPairs;
    public List<Button> allButons;
    List<GameObject> leftButtonsObjects;
    List<GameObject> rightButtonsObjects;
    public List<int> inConnectionButtonIndexes;
    public List<Connection> connections;
    int selectedButtonIndex = -1;
    int selectedButtonPairIndex = -1;
    int selectedButtonDuality = -1;
    int challengeId = 6;
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;

    void Start()
    {
        correctPairStrings = new List<string>();
        allButons = new List<Button>();
        leftButtonsObjects = new List<GameObject>();
        rightButtonsObjects = new List<GameObject>();
        correctPairs = new List<Pair>();
        inConnectionButtonIndexes = new List<int>();
        connections = new List<Connection>();

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
            correctPairStrings.Add(reader.GetString(3));
        }

        for (int i = 0; i < correctPairStrings.Count; i++)
        {
            string[] pairStrings = correctPairStrings[i].Split(new string[] { " // " }, System.StringSplitOptions.None);
            correctPairs.Add(new Pair(pairStrings[0], pairStrings[1]));
            //LEFT BUTTON
            GameObject pairButtonLeft = (GameObject)Instantiate(pairButtonPrefab);
            pairButtonLeft.GetComponentInChildren<TextMeshProUGUI>().text = pairStrings[0];
            int pairIndex = i;
            int buttonIndex1 = allButons.Count;
            Button tempButtonLeft = pairButtonLeft.GetComponent<Button>();
            tempButtonLeft.onClick.AddListener(() => OptionButtonClicked(buttonIndex1, pairIndex, 0));
            leftButtonsObjects.Add(pairButtonLeft);
            allButons.Add(tempButtonLeft);

            //RIGHT BUTTON
            GameObject pairButtonRight = (GameObject)Instantiate(pairButtonPrefab);
            pairButtonRight.GetComponentInChildren<TextMeshProUGUI>().text = pairStrings[1];
            int buttonIndex2 = allButons.Count;
            Button tempButtonRight = pairButtonRight.GetComponent<Button>();
            tempButtonRight.onClick.AddListener(() => OptionButtonClicked(buttonIndex2, pairIndex, 1));
            rightButtonsObjects.Add(pairButtonRight);
            allButons.Add(tempButtonRight);
        }
        DrawButtons();
        checkButton.onClick.AddListener(() => CheckButtonClicked());
    }

    // Update is called once per frame
    void Update()
    {
        if(connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                UpdateConnectionLineY(connections[i]);
            }
        }
    }

    void OptionButtonClicked(int buttonIndex, int pairIndex, int duality)
    {
        if (inConnectionButtonIndexes.Contains(buttonIndex))
        {
            //Em conexão
        }
        else
        {
            if (selectedButtonIndex == -1)
            {
                selectedButtonIndex = buttonIndex;
                selectedButtonPairIndex = pairIndex;
                selectedButtonDuality = duality;
                allButons[buttonIndex].GetComponentInChildren<Image>().color = Color.red;
            }
            else
            {
                if (buttonIndex == selectedButtonIndex)
                {
                    selectedButtonIndex = -1;
                    selectedButtonPairIndex = -1;
                    selectedButtonDuality = -1;
                    allButons[buttonIndex].GetComponentInChildren<Image>().color = Color.white;
                }
                else if (selectedButtonDuality == duality)
                {
                    allButons[selectedButtonIndex].GetComponentInChildren<Image>().color = Color.white;
                    selectedButtonIndex = buttonIndex;
                    selectedButtonPairIndex = pairIndex;
                    selectedButtonDuality = duality;
                    allButons[buttonIndex].GetComponentInChildren<Image>().color = Color.red;
                }
                else if (selectedButtonDuality != duality)
                {
                    allButons[selectedButtonIndex].GetComponentInChildren<Image>().color = Color.yellow;
                    allButons[buttonIndex].GetComponentInChildren<Image>().color = Color.yellow;
                    inConnectionButtonIndexes.Add(selectedButtonIndex);
                    inConnectionButtonIndexes.Add(buttonIndex);
                    float leftX;
                    float leftY;
                    float rightX;
                    float rightY;
                    if (selectedButtonDuality == 0)
                    {
                        leftX = scrollListViewport.InverseTransformPoint(allButons[selectedButtonIndex].GetComponent<RectTransform>().position).x;
                        rightX = scrollListViewport.InverseTransformPoint(allButons[buttonIndex].GetComponent<RectTransform>().position).x;
                        leftX = leftX + allButons[selectedButtonIndex].GetComponent<RectTransform>().sizeDelta.x / 2 - 5;
                        rightX = rightX - allButons[buttonIndex].GetComponent<RectTransform>().sizeDelta.x / 2 + 5;

                        leftY = scrollListViewport.InverseTransformPoint(allButons[selectedButtonIndex].GetComponent<RectTransform>().position).y;
                        rightY = scrollListViewport.InverseTransformPoint(allButons[buttonIndex].GetComponent<RectTransform>().position).y;
                    }
                    else
                    {
                        leftX = scrollListViewport.InverseTransformPoint(allButons[buttonIndex].GetComponent<RectTransform>().position).x;
                        rightX = scrollListViewport.InverseTransformPoint(allButons[selectedButtonIndex].GetComponent<RectTransform>().position).x;
                        leftX = leftX + allButons[buttonIndex].GetComponent<RectTransform>().sizeDelta.x / 2 - 5;
                        rightX = rightX - allButons[selectedButtonIndex].GetComponent<RectTransform>().sizeDelta.x / 2 + 5;

                        leftY = scrollListViewport.InverseTransformPoint(allButons[buttonIndex].GetComponent<RectTransform>().position).y;
                        rightY = scrollListViewport.InverseTransformPoint(allButons[selectedButtonIndex].GetComponent<RectTransform>().position).y;
                    }
                    //create connection and line
                    GameObject lineRenderer = (GameObject)Instantiate(lineRendererPrefab);
                    lineRenderer.transform.SetParent(scrollListViewport, false);
                    lineRenderer.transform.localScale = new Vector3(1, 1, 1);
                    UnityEngine.UI.Extensions.UILineRenderer line = lineRenderer.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>();

                    var leftPoint = new Vector2() { x = leftX, y = leftY };
                    var rightPoint = new Vector2() { x = rightX, y = rightY };
                    var pointList = new List<Vector2>();
                    pointList.Add(leftPoint);
                    pointList.Add(rightPoint);
                    line.Points = pointList.ToArray();
                    ParentPanel.SetAsLastSibling();
                    Connection connection;
                    if (selectedButtonDuality == 0)
                    {
                        connection = new Connection(selectedButtonIndex, selectedButtonPairIndex, buttonIndex, pairIndex, line);
                    }
                    else
                    {
                        connection = new Connection(buttonIndex, pairIndex, selectedButtonIndex, selectedButtonPairIndex, line);
                    }
                    connections.Add(connection);


                    selectedButtonIndex = -1;
                    selectedButtonPairIndex = -1;
                    selectedButtonDuality = -1;
                }
            }
        }
    }

    void CheckButtonClicked()
    {
        List<int> rightPairsIndexes = new List<int>();
        List<int> missedPairsIndexes = new List<int>();
        int nMissedPairs = 0;
        for(int i = 0; i < connections.Count; i++)
        {
            if (connections[i].leftButtonPairIndex == connections[i].rightButtonPairIndex)
            {
                rightPairsIndexes.Add(connections[i].leftButtonPairIndex);
            }
        }

        for(int i = 0; i < correctPairs.Count; i++)
        {
            if(!rightPairsIndexes.Contains(i))
            {
                nMissedPairs++;
                Debug.Log("Missed Pair! Correct-> "+ correctPairs[i].getLeft() + " - " + correctPairs[i].getRight());
                missedPairsIndexes.Add(i);
            }
        }
        if(nMissedPairs == 0)
        {
            Debug.Log("ALL CORRECT!");
        }
    }

    void DrawButtons()
    {
        leftButtonsObjects.Shuffle();
        rightButtonsObjects.Shuffle();
        for (int i = 0; i < leftButtonsObjects.Count; i++)
        {
            leftButtonsObjects[i].transform.SetParent(ParentPanel, false);
            leftButtonsObjects[i].transform.localScale = new Vector3(1, 1, 1);

            rightButtonsObjects[i].transform.SetParent(ParentPanel, false);
            rightButtonsObjects[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public void removeConnection(Connection connection)
    {
        Destroy(connection.line);
        allButons[connection.leftButtonIndex].GetComponent<Image>().color = Color.white;
        allButons[connection.rightButtonIndex].GetComponent<Image>().color = Color.white;
        inConnectionButtonIndexes.Remove(connection.leftButtonIndex);
        inConnectionButtonIndexes.Remove(connection.rightButtonIndex);
        connections.Remove(connection);
    }

    public void UpdateConnectionLineY(Connection connection)
    {
        float leftX = connection.line.Points[0].x;
        float rightX = connection.line.Points[1].x;
        float leftY = scrollListViewport.InverseTransformPoint(allButons[connection.leftButtonIndex].GetComponent<RectTransform>().position).y;
        float rightY = scrollListViewport.InverseTransformPoint(allButons[connection.rightButtonIndex].GetComponent<RectTransform>().position).y;
        var leftPoint = new Vector2() { x = leftX, y = leftY };
        var rightPoint = new Vector2() { x = rightX, y = rightY };
        var pointList = new List<Vector2>();
        pointList.Add(leftPoint);
        pointList.Add(rightPoint);
        connection.line.Points = pointList.ToArray();
    }
}


public class Pair
{
    public string[] pairArray = new string[2]; 

    public  Pair(string left, string right)
    {
        pairArray[0] = left;
        pairArray[1] = right;
    }

    public string getLeft()
    {
        return pairArray[0];
    }

    public string getRight()
    {
        return pairArray[1];
    }


}

public class Connection 
{

    public int leftButtonIndex;
    public int rightButtonIndex;
    public int leftButtonPairIndex;
    public int rightButtonPairIndex;
    public UnityEngine.UI.Extensions.UILineRenderer line;

    public Connection(int leftButtonIndex, int leftButtonPairIndex, int rightButtonIndex, int rightButtonPairIndex, UnityEngine.UI.Extensions.UILineRenderer line)
    {
        this.leftButtonIndex = leftButtonIndex;
        this.leftButtonPairIndex = leftButtonPairIndex;
        this.rightButtonIndex = rightButtonIndex;
        this.rightButtonPairIndex = rightButtonPairIndex;
        this.line = line;
    }

}