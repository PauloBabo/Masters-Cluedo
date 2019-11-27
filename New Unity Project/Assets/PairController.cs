using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PairController : MonoBehaviour
{
    public GameObject pairButtonPrefab;
    public RectTransform ParentPanel;
    string pair1String = "Emanuel // Bima";
    string pair2String = "Paulo // Natureza";
    string pair3String = "Xico // Xerife";
    List<string> correctPairStrings;
    List<Pair> correctPairs;
    List<Pair> madePairs;
    List<GameObject> leftButtons;
    List<GameObject> rightButtons;

    void Start()
    {
        correctPairStrings = new List<string>();
        correctPairs = new List<Pair>();
        leftButtons = new List<GameObject>();
        rightButtons = new List<GameObject>();
        correctPairStrings.Add(pair1String);
        correctPairStrings.Add(pair2String);
        correctPairStrings.Add(pair3String);
        for (int i = 0; i < correctPairStrings.Count; i++)
        {
            string[] pairStrings = correctPairStrings[i].Split(new string[] { " // " }, System.StringSplitOptions.None);
            correctPairs.Add(new Pair(pairStrings[0], pairStrings[1]));

            //leftButton
            GameObject pairButtonLeft = (GameObject)Instantiate(pairButtonPrefab);
            pairButtonLeft.GetComponentInChildren<TextMeshProUGUI>().text = pairStrings[0];
            int index = i;
            Button tempButtonLeft = pairButtonLeft.GetComponent<Button>();
            //tempButtonRight.onClick.AddListener(() => OptionButtonClicked(index, 0));
            leftButtons.Add(pairButtonLeft);

            //rightButton
            GameObject pairButtonRight = (GameObject)Instantiate(pairButtonPrefab);
            pairButtonRight.GetComponentInChildren<TextMeshProUGUI>().text = pairStrings[1];
            int index2 = i;
            Button tempButtonRight = pairButtonRight.GetComponent<Button>();
            //tempButtonRight.onClick.AddListener(() => OptionButtonClicked(index2, 1));
            rightButtons.Add(pairButtonRight);
        }
        DrawButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawButtons()
    {
        leftButtons.Shuffle();
        rightButtons.Shuffle();
        for (int i = 0; i < leftButtons.Count; i++)
        {
            leftButtons[i].transform.SetParent(ParentPanel, false);
            leftButtons[i].transform.localScale = new Vector3(1, 1, 1);

            rightButtons[i].transform.SetParent(ParentPanel, false);
            rightButtons[i].transform.localScale = new Vector3(1, 1, 1);
        }
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

public class DualityButton
{
    Button button;
    int duality; //0 esquerda - 1 direita
    int pairIndex;
    public DualityButton(int pairIndex, Button button, int duality)
    {
        this.button = button;
        this.duality = duality;
    }
}