using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MyHandScript : MonoBehaviour, IDropHandler
{
    public GameObject MyPlayer;
    public int MyPlayerNum;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            CardScript DragingObj;
            if(eventData.pointerDrag.TryGetComponent<CardScript>(out DragingObj) == false) 
            {
                eventData.pointerDrag.transform.parent = NetworkManager.networkManager.MyHand.transform;
                return;
            }
            eventData.pointerDrag.transform.parent = transform;
        }
    }
    
    private void Start() {
       MyPlayerNum = int.Parse(MyPlayer.name.Replace("P",""));
    }

    private void Update() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CardScript>().MyOwnerNum = MyPlayerNum;
        }
        
    }
}
