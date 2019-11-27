using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private string[] options;
    private bool[] selected;
    private Button[] choiceButtons;
    private List<int> correctIndexes;
    // Start is called before the first frame update
    void Start()
    {
        question = "Quais das seguintes cores são primárias?";
        options = new string[5];
        selected = new bool[5];
        correctIndexes = new List<int>();
        choiceButtons = new Button[5];
        correctIndexes.Add(1);
        correctIndexes.Add(2);
        selected[0] = false;
        selected[1] = false;
        selected[2] = false;
        selected[3] = false;
        selected[4] = false;
        options[0] = "Laranja";
        options[1] = "Amarelo";
        options[2] = "Azul";
        options[3] = "Dourado";
        options[4] = "Cor De Bima";
        questionText.text = question;

        checkButton.onClick.AddListener(() => CheckButtonClicked());
        for (int i = 0; i < options.Length; i++)
        {
            GameObject choiceButton = (GameObject)Instantiate(choiceButtonPrefab);
            choiceButton.transform.SetParent(ParentPanel, false);
            choiceButton.transform.localScale = new Vector3(1, 1, 1);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            Button tempButton = choiceButton.GetComponent<Button>();
            int index = i;
            tempButton.onClick.AddListener(() => ChoiceButtonClicked(index));
            choiceButtons[i] = tempButton;
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
        for (int i = 0; i < selected.Length; i++)
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
