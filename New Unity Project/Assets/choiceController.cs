using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class choiceController : MonoBehaviour
{
    string question;
    string[] options = new string[4];
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

    // Start is called before the first frame update
    void Start()
    {
        //TODO get pergunta info from database
        question = "Qual é o primeiro nome do Bima?";
        options[0] = "Emanuka";
        options[1] = "Ivo";
        options[2] = "Emanuel";
        options[3] = "Bima";
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
