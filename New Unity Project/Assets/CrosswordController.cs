using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CrosswordController : MonoBehaviour
{
    public GameObject letterInputPrefab;
    public GameObject transparentButtonPrefab;
    public RectTransform scrollListContentTransform;
    public TextMeshProUGUI hintText;
    List<string> hintsAndWords = new List<string>();
    Dictionary<string,string> wordHintMap = new Dictionary<string, string>();
    List<string> words = new List<string>();
    GameObject[,] gridCells;
    public GameObject scrollListContent;
    Crossword crossword;
    
    // Start is called before the first frame update
    void Start()
    {
        
        string wordHint1 = "wordHint1:period";
        string wordHint2 = "wordHint2:meaning";
        string wordHint3 = "wordHint3:terms";
        string wordHint4 = "wordHint4:rush";
        string wordHint5 = "wordHint5:teach";
        string wordHint6 = "wordHint6:deal";
        string wordHint7 = "wordHint7:socialism";
        string wordHint8 = "wordHint8:technology";
        string wordHint9 = "wordHint9:battery";
        string wordHint10 = "wordHint10:winner";
        string wordHint11 = "wordHint11:cheap";
        string wordHint12 = "wordHint12:posture";
        string wordHint13 = "wordHint13:night";
        string wordHint14 = "wordHint14:confidence";
        string wordHint15 = "wordHint15:person";

        hintsAndWords.Add(wordHint1);
        hintsAndWords.Add(wordHint2);
        hintsAndWords.Add(wordHint3);
        hintsAndWords.Add(wordHint4);
        hintsAndWords.Add(wordHint5);
        hintsAndWords.Add(wordHint6);
        hintsAndWords.Add(wordHint7);
        hintsAndWords.Add(wordHint8);
        hintsAndWords.Add(wordHint9);
        hintsAndWords.Add(wordHint10);
        hintsAndWords.Add(wordHint11);
        hintsAndWords.Add(wordHint12);
        hintsAndWords.Add(wordHint13);
        hintsAndWords.Add(wordHint14);
        hintsAndWords.Add(wordHint15);
        
       

        for (int i = 0; i < hintsAndWords.Count; i++)
        {
            string[] parts = hintsAndWords[i].Split(':');
            wordHintMap.Add(parts[1].ToUpper(), parts[0]);
            words.Add(parts[1].ToUpper());
        }

        crossword = new Crossword(words);
        crossword.Init();
        crossword.DisplayWordGrid();

        GridLayoutGroup  gridLayout = scrollListContent.GetComponent<GridLayoutGroup>();
        gridLayout.constraintCount = crossword.endingX - crossword.startingX + 1;

        gridCells = new GameObject[crossword.endingY - crossword.startingY + 1, crossword.endingX - crossword.startingX + 1];
        for (int y = 0; y < gridCells.GetLength(0); y++)
        {
            for (int x = 0; x < gridCells.GetLength(1); x++)
            {
                gridCells[y, x] = new GameObject();
            }
        }

        for (int i = crossword.startingX; i <= crossword.endingX; i++)
        {
            gridCells[0, i - crossword.startingX]  = (GameObject)Instantiate(transparentButtonPrefab);
            gridCells[0, i - crossword.startingX].transform.SetParent(scrollListContentTransform, false);
            gridCells[0, i - crossword.startingX].transform.localScale = new Vector3(1, 1, 1);
            
        }

        for (int y = crossword.startingY; y < crossword.endingY; y++)
        {
            gridCells[y - crossword.startingY + 1, 0] = (GameObject)Instantiate(transparentButtonPrefab);
            gridCells[y - crossword.startingY + 1, 0].transform.SetParent(scrollListContentTransform, false);
            gridCells[y - crossword.startingY + 1, 0].transform.localScale = new Vector3(1, 1, 1);


            for (int x = crossword.startingX; x < crossword.endingX; x++)
            {
                if (crossword.wordGrid[y, x] != '\0' && crossword.wordGrid[y, x] != '-')
                {
                    GameObject inputObject = (GameObject)Instantiate(letterInputPrefab);
                    inputObject.transform.SetParent(scrollListContentTransform, false);
                    inputObject.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    gridCells[y - crossword.startingY + 1, x - crossword.startingX + 1] = (GameObject)Instantiate(transparentButtonPrefab);
                    gridCells[y - crossword.startingY + 1, x - crossword.startingX + 1].transform.SetParent(scrollListContentTransform, false);
                    gridCells[y - crossword.startingY + 1, x - crossword.startingX + 1].transform.localScale = new Vector3(1, 1, 1);
                    
                }
            }
        }

        for (int i = 0; i < crossword.placedWords.Count; i++)
        {
            Debug.Log(crossword.placedWords.Count);

            if (crossword.placedWords[i].horientation == 'H')
            {
                gridCells[(int)(crossword.placedWords[i].startPosition.y - crossword.startingY) + 1, (int)crossword.placedWords[i].startPosition.x - crossword.startingX ].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
                int index = i;
                gridCells[(int)(crossword.placedWords[i].startPosition.y - crossword.startingY) + 1, (int)crossword.placedWords[i].startPosition.x - crossword.startingX].GetComponent<Button>().onClick.AddListener(() => IndexButtonClicked(index));
            }
            else
            {
                gridCells[(int)(crossword.placedWords[i].startPosition.y - crossword.startingY) , (int)crossword.placedWords[i].startPosition.x - crossword.startingX + 1].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
                int index = i;
                gridCells[(int)(crossword.placedWords[i].startPosition.y - crossword.startingY), (int)crossword.placedWords[i].startPosition.x - crossword.startingX + 1].GetComponent<Button>().onClick.AddListener(() => IndexButtonClicked(index));
            }
        }
    }

    void IndexButtonClicked(int index)
    {
        if(crossword.placedWords[index].horientation == 'H')
            hintText.text = (index + 1).ToString() + " : Horizontal ->  " + wordHintMap[crossword.placedWords[index].wordString];
        else
            hintText.text = (index + 1).ToString() + " : Vertical ->  " + wordHintMap[crossword.placedWords[index].wordString];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}


public class CrosswordGenerator
{
    
}

public class Crossword
{
    public string[,] hintGrid = new string[50,50];
    public int[,] numberGrid = new int[51, 51];
    public char[,] wordGrid = new char[50,50];
    public List<Word> placedWords = new List<Word>();
    public List<string> remainingStrings;
    public int startingX = 1000;
    public int startingY = 1000;
    public int endingX = -1;
    public int endingY = -1;
    int wordNumber = 1;

    public Crossword(List<string> words)
    {
        this.remainingStrings = SortByLength(words).ToList();
    }

    public void Init()
    {
        PlaceWord(remainingStrings[0], 20, 25, 'H');
        int currentWordIndex = 0;
        while (remainingStrings.Count > 0 )
        {
            if (currentWordIndex >= remainingStrings.Count)
            {
                break;
            }
            string currentWord = remainingStrings[currentWordIndex];
            bool placed = false;
            for (int p = 0; p < placedWords.Count; p++)
            {
                List<int[]> sameIndexes = HasCommonLetter(currentWord, placedWords[p].wordString);
                if (sameIndexes.Count > 0) 
                {
                    int x = -1;
                    int y = -1;
                    for (int i = 0; i < sameIndexes.Count; i++)
                    {
                        if (placedWords[p].horientation == 'H')
                        {
                            x = (int)(placedWords[p].startPosition.x + sameIndexes[i][1]);
                            y = (int)(placedWords[p].startPosition.y - sameIndexes[i][0]);
                            if(CanBePlaced(currentWord,x,y,'V'))
                            {
                                PlaceWord(currentWord, x, y, 'V');
                                placed = true;
                                break;
                            }
                        }
                        else
                        {
                            x = (int)(placedWords[p].startPosition.x - sameIndexes[i][0]);
                            y = (int)(placedWords[p].startPosition.y + sameIndexes[i][1]);
                            if (CanBePlaced(currentWord, x, y, 'H'))
                            {
                                PlaceWord(currentWord, x, y, 'H');
                                placed = true;
                                break;
                            }
                        }
                    }
                }
                if (placed)
                {
                    
                    break;
                }
            }
            if (!placed)
            {
                currentWordIndex++;
            }
            else
            {
                currentWordIndex = 0;
            }
        }
    }

    public bool CanBePlaced(string wordToPlace, int x, int y, char horientation)
    {
        if (x < 0 || y < 0 || x >= wordGrid.GetLength(1) || y >= wordGrid.GetLength(0))
        {
            return false;
        }
        if (horientation == 'H')
        {
            if (x + wordToPlace.Length >= wordGrid.GetLength(1))
            {
                return false;        
            }
            if (x - 1 >= 0)
            {
                if (wordGrid[y, x - 1] != '\0')
                {
                    return false;
                }

            }
            if (x + wordToPlace.Length < wordGrid.GetLength(1))
            {
                if(wordGrid[y, x + wordToPlace.Length] != '\0')
                {
                    return false;
                }

            }
            for (int i = 0; i < wordToPlace.Length; i++)
            {
                if (wordGrid[y, x + i] != '\0')
                {
                    if (wordGrid[y, x + i] != wordToPlace[i])
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            if (y + wordToPlace.Length >= wordGrid.GetLength(0))
            {
                return false;
            }
            if (y - 1 >= 0)
            {
                if (wordGrid[y - 1, x] != '\0')
                {
                    return false;
                }

            }
            if (y + wordToPlace.Length < wordGrid.GetLength(0))
            {
                if (wordGrid[y + wordToPlace.Length , x ] != '\0')
                {
                    return false;
                }

            }
            for (int i = 0; i < wordToPlace.Length; i++)
            {
                if (wordGrid[y + i, x] != '\0')
                {
                    if (wordGrid[y + i, x] != wordToPlace[i])
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool PlaceWord(string wordString, int x, int y, char horientation)
    {
        numberGrid[y, x] = wordNumber;
        wordNumber++;
        if (x < startingX)
        {
            startingX = x;
        }
        if(y < startingY)
        {
            startingY = y;
        }
        if(horientation == 'H')
        {
            if (x + wordString.Length > endingX)
            {
                endingX = x + wordString.Length;        
            }
            if (y > endingY)
            {
                endingY = y;
            }

            if (x - 1 >= 0)
            {
                wordGrid[y, x - 1] = '-';

            }
            if (x + wordString.Length < wordGrid.GetLength(1))
            {
                wordGrid[y, x + wordString.Length] = '-';
               
            }
            for (int i = 0; i < wordString.Length; i++)
            {
                wordGrid[y, x + i] = wordString[i];

                if (hintGrid[y, x + i] == null)
                {
                    hintGrid[y, x + i] = wordString;
                }
                else
                {
                    hintGrid[y, x + 1] += "/" + wordString;
                }
            }
        }
        else
        {
            if (y + wordString.Length > endingY)
            {
                endingY = y + wordString.Length;
            }
            if (x > endingX)
            {
                endingX = x;
            }

            if (y - 1 >= 0)
            {
                wordGrid[y - 1, x] = '-';
                
            }
            if (y + wordString.Length < wordGrid.GetLength(0))
            {
               wordGrid[y + wordString.Length, x] = '-';
            }
            for (int i = 0; i < wordString.Length; i++)
            {
                wordGrid[y + i, x] = wordString[i];

                if (hintGrid[y + i, x] == null)
                {
                    hintGrid[y + i, x] = wordString;
                }
                else
                {
                    hintGrid[y + i, x] += "/" + wordString;
                }
            }
        }
        remainingStrings.Remove(wordString);
        placedWords.Add(new Word(wordString, new Vector2(x, y), horientation));
        return true;
    }

    public void DisplayWordGrid()
    {
        string gridString = "";

        Debug.Log(wordGrid.GetLength(0));

        for (int y = startingY; y < endingY; y++)
        {
            for (int x = startingX; x < endingX; x++)
            {
                if (wordGrid[y,x] == '\0')
                {
                    gridString += "X ";
                }
                else
                {
                    gridString += wordGrid[y, x] + " ";
                }
            }
            gridString += "\n";
        }
        Debug.Log(gridString);
        for (int i = 0; i < remainingStrings.Count; i++)
        {
            Debug.Log("Remains: " + remainingStrings[i]);
        }
    }

    static IEnumerable<string> SortByLength(IEnumerable<string> e)
    {
        var sorted = from s in e
                     orderby s.Length descending
                     select s;
        return sorted;
    }

    public List<int[]> HasCommonLetter(string wordToPlace, string placedWord)
    {
        List<int[]> sameLetterIndexes = new List<int[]>();

        for (int i = 0; i < wordToPlace.Length; i++)
        {
            for (int j = 0; j < placedWord.Length; j++)
            {
                if (wordToPlace[i] == placedWord[j])
                {
                    int[] indexes = new int[2];
                    indexes[0] = i;
                    indexes[1] = j;
                    sameLetterIndexes.Add(indexes);
                }
            }
        }
        return sameLetterIndexes;
    }
}

public class Word
{
    public char horientation;
    public string wordString;
    public Vector2 startPosition;

    public Word(string wordString, Vector2 startPosition, char horientation)
    {
        this.wordString = wordString;
        this.startPosition = startPosition;
        this.horientation = horientation;
    }
}