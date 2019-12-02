using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuScene : MonoBehaviour
{
    public Button adventureButton;
    // Start is called before the first frame update
    void Start()
    {
        adventureButton.GetComponent<Button>().onClick.AddListener(() => AdventureButtonClicked());
    }

    void AdventureButtonClicked()
    {
        SceneManager.LoadScene("chapterSelectionScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
