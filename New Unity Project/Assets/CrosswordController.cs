using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrosswordController : MonoBehaviour
{
    List<string> hintsAndWords = new List<string>();
    Dictionary<string,string> wordHintMap = new Dictionary<string, string>();
    List<string> words = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
        string wordHint1 = "wordHint1:period";
        string wordHint2 = "wordHint2:meaning";
        string wordHint3 = "wordHint3:terms";
        string wordHint4 = "wordHint4:rush";
        string wordHint5 = "wordHint5:teach";
        string wordHint6 = "wordHint6:deal";
        string wordHint7 = "wordHint7:friend";
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
            wordHintMap.Add(parts[1], parts[0]);
            words.Add(parts[1].ToUpper());
        }

        Crossword crossword = new Crossword(words);
        crossword.Init();
        crossword.DisplayWordGrid();
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
    string[,] hintGrid = new string[50,50];
    char[,] wordGrid = new char[50,50];
    List<Word> placedWords = new List<Word>();
    List<string> remainingStrings;


    public Crossword(List<string> words)
    {
        this.remainingStrings = SortByLength(words).ToList();
    }

    public void Init()
    {
        placeWord(remainingStrings[0], 20, 25, 'H');
        int currentWordIndex = 0;
        while (remainingStrings.Count > 0 )
        {
            string currentWord = remainingStrings[currentWordIndex];
            bool placed = false;
            for (int p = 0; p < placedWords.Count; p++)
            {
                List<int[]> sameIndexes = HasCommonLetter(currentWord, placedWords[p].wordString);
                if (sameIndexes.Count > 0) //TODO check if remaining strings cant be placed
                {
                    int x = -1;
                    int y = -1;
                    for (int i = 0; i < sameIndexes.Count; i++)
                    {
                        if (placedWords[p].horientation == 'H')
                        {
                            x = (int)(placedWords[p].startPosition.x + sameIndexes[i][1]);
                            y = (int)(placedWords[p].startPosition.y - sameIndexes[i][0]);
                            //todo check if can be placed
                            placeWord(currentWord, x, y, 'V');
                            placed = true;
                            break;
                        }
                        else
                        {
                            x = (int)(placedWords[p].startPosition.x - sameIndexes[i][0]);
                            y = (int)(placedWords[p].startPosition.y + sameIndexes[i][1]);
                            //todo check if can be placed
                            placeWord(currentWord, x, y, 'H');
                            placed = true;
                            break;
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

    public bool placeWord(string wordString, int x, int y, char horientation)
    {
        //TODO if(CanPlace())
        if(horientation == 'H')
        {
            for(int i = 0; i < wordString.Length; i++)
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
        placedWords.Add(new Word(wordString, new Vector2(x, y), horientation));
        remainingStrings.Remove(wordString);
        return true;
    }

    public void DisplayWordGrid()
    {
        string gridString = "";

        Debug.Log(wordGrid.GetLength(0));

        for (int y = 0; y < wordGrid.GetLength(0); y++)
        {
            for (int x = 0; x < wordGrid.GetLength(1); x++)
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
    }

    static IEnumerable<string> SortByLength(IEnumerable<string> e)
    {
        // Use LINQ to sort the array received and return a copy.
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