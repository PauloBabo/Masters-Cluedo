using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PairButtonBehaviour : MonoBehaviour, IPointerClickHandler
{
    int tap;
    PairController pairController;

    void Start()
    {
       pairController = FindObjectOfType<PairController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tap = eventData.clickCount;
        
        if (tap == 2)
        {
            
            int index = -1;
            for (int i = 0; i < pairController.allButons.Count; i++)
            {
                if(pairController.allButons[i].Equals(this.gameObject.GetComponent<Button>()))
                {
                    index = i;
                    break;
                }
            }
            if (pairController.inConnectionButtonIndexes.Contains(index))
            {
                for(int i = 0; i< pairController.connections.Count; i++)
                {
                    if(pairController.connections[i].leftButtonIndex == index|| pairController.connections[i].rightButtonIndex == index)
                    {
                        pairController.removeConnection(pairController.connections[i]);
                        Debug.Log("Connection destroyed");
                    }
                }
            }
        }

    }
}