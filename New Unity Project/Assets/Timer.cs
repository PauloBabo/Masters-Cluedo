using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeLeft;
    public bool counting = false;
    public GameObject groupController;

    void Start()
    {
        timeLeft = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(counting)
        {
            timeLeft -= Time.deltaTime;
            gameObject.GetComponent<TextMeshProUGUI>().text = timeLeft.ToString("F0");
            if (timeLeft <= 0)
            {
                groupController.GetComponent<GroupController>().QuestionTimeOut();
            }
        }
       
    }

    public void Reset()
    {
        timeLeft = 5;
    }

}
