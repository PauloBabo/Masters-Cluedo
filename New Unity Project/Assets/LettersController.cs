using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LettersController : MonoBehaviour
{
    public GameObject optionButtonPrefab;
    public GameObject socketButtonPrefab;
    public RectTransform horizontalPanelPrefab;
    public RectTransform answerVerticalPanel;
    public RectTransform optionsPanel;
    public TextMeshProUGUI questionText;
    public Button checkButton;
    string question;
    string correctWord;
    string[] correctWords;
    List<int> selectedIndexes;
    List<bool> selectedOptionButtons;
    List<Button> socketButtons;
    List<Button> optionButtons;
    List<char> optionLetters;
    string correctWordNoSpaces;
    
    // Start is called before the first frame update
    void Start()
    {
        question = "What is the powerhouse of the cell?";
        correctWord = "MitochondriA";

        question = question.Replace("(_)", "_________");
        correctWords = correctWord.Split(' ');
        Debug.Log("Number of words:" + correctWords.Length);
        questionText.text = question;
        //create letter sockets
        selectedIndexes = new List<int>();
        socketButtons = new List<Button>();
        for (int i = 0; i < correctWords.Length; i++)
        {
            RectTransform horizontalPanel = (RectTransform)Instantiate(horizontalPanelPrefab);
            horizontalPanel.transform.SetParent(answerVerticalPanel, false);
            horizontalPanel.transform.localScale = new Vector3(1, 1, 1);
            for (int j = 0; j < correctWords[i].Length; j++)
            {
                selectedIndexes.Add(-1);
                GameObject letterSocket = (GameObject)Instantiate(socketButtonPrefab);
                if (correctWords[i].Length > 9)
                {
                    float space = correctWords[i].Length * 20;
                    float socketWidth = (1000 - space) / correctWords[i].Length;
                    letterSocket.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, socketWidth);
                }
                letterSocket.transform.SetParent(horizontalPanel, false);
                Button tempSocket = letterSocket.GetComponent<Button>();
                int index = socketButtons.Count;
                tempSocket.onClick.AddListener(() => SocketButtonClicked(index));
                socketButtons.Add(tempSocket);
            }
        }

        //create option buttons
        correctWordNoSpaces = correctWord.Replace(" ", "");
        correctWordNoSpaces = correctWordNoSpaces.ToUpper();
        optionLetters = new List<char>();
        for (int i = 0; i < correctWordNoSpaces.Length; i++)
        {
            optionLetters.Add(correctWordNoSpaces[i]);
        }
        int nAdicionalLetters = Mathf.CeilToInt(correctWordNoSpaces.Length/2.5f);
        for (int i = 0; i < nAdicionalLetters; i++)
        {
            optionLetters.Add(GetRandomLetter());
        }
        optionLetters.Shuffle();
        optionButtons = new List<Button>();
        selectedOptionButtons = new List<bool>();
        for (int i = 0; i < optionLetters.Count; i++)
        {
            GameObject optionButton= (GameObject)Instantiate(optionButtonPrefab);
            optionButton.transform.SetParent(optionsPanel, false);
            optionButton.transform.localScale = new Vector3(1, 1, 1);
            optionButton.GetComponentInChildren<TextMeshProUGUI>().text = optionLetters[i].ToString();
            Button tempButton = optionButton.GetComponentInChildren<Button>();
            int index = i;
            tempButton.onClick.AddListener(() => OptionButtonClicked(index));
            selectedOptionButtons.Add(false);
            optionButtons.Add(tempButton);
        }
        checkButton.GetComponent<Button>().onClick.AddListener(() => CheckButtonClicked());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OptionButtonClicked(int index)
    {
        if (!selectedOptionButtons[index])
        {
            bool updated = false;
            for (int i = 0; i < selectedIndexes.Count; i++)
            {
                if (selectedIndexes[i] == -1)
                {
                    selectedIndexes[i] = index;
                    socketButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionButtons[index].GetComponentInChildren<TextMeshProUGUI>().text;
                    updated = true;
                    break;
                }
            }
            if (updated)
            {
                selectedOptionButtons[index] = true;
                optionButtons[index].GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            for (int i = 0; i < selectedIndexes.Count; i++)
            {
                if (selectedIndexes[i] == index)
                {
                    selectedIndexes[i] = -1;
                    socketButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                   
                }
            }
            optionButtons[index].GetComponent<Image>().color = Color.white;
            selectedOptionButtons[index] = false;
        }
    }

    void SocketButtonClicked(int index)
    {
        if (selectedIndexes[index] != -1)
        {
            socketButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = "";
            optionButtons[selectedIndexes[index]].GetComponent<Image>().color = Color.white;
            selectedOptionButtons[selectedIndexes[index]] = false;
            selectedIndexes[index] = -1;
        }
    }

    void CheckButtonClicked()
    {
        string answerString = "";
        for (int i = 0; i < socketButtons.Count; i++)
        {
            answerString += socketButtons[i].GetComponentInChildren<TextMeshProUGUI>().text;
        }

        if (answerString.Equals(correctWordNoSpaces))
        {
            Debug.Log("Correct answer");
        }
        else
        {
            Debug.Log("Incorrect answer");
        }

        //TODO verificar se preencheu as sockets todas?
    }

    char GetRandomLetter()
    {
        string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char c = st[Random.Range(0,st.Length)];
        return c;
    }
}

// https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
public static class IListExtensions
{
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

