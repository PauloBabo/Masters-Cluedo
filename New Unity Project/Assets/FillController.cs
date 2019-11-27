using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    string[] options;
    Button[] choiceButtons;
    int[] correctIndexes;
    int[] selectedIndexes;
    bool[] selectedButtons;
    // Start is called before the first frame update
    void Start()
    {
        question = "The (_) is the (_) of the cell!";
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


        Debug.Log("Espaços: " + nSpaces);
        Debug.Log("Questoes: " + questionTexts.Length);

        int nOptions = 6;
        options = new string[nOptions];
        options[0] = "Cromossome";
        options[1] = "RNA";
        options[2] = "DNA";
        options[3] = "Ribossome";
        options[4] = "Power-house";
        options[5] = "Mitocondrea";

        selectedButtons = new bool[nOptions];
        for (int i = 0; i < selectedButtons.Length; i++)
            selectedButtons[i] = false;

        correctIndexes = new int[nSpaces];
        correctIndexes[0] = 5;
        correctIndexes[1] = 4;


        selectedIndexes = new int[nSpaces];
        for (int i = 0; i < selectedIndexes.Length; i++)
            selectedIndexes[i] = -1;

        choiceButtons = new Button[nOptions];
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
            for (int i = 0; i < selectedIndexes.Length; i++)
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
            for (int i = 0; i < selectedIndexes.Length; i++)
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
        for (int i = 0; i < correctIndexes.Length; i++)
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
