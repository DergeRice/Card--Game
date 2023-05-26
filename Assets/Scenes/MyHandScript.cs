using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MyHandScript : MonoBehaviour, IDropHandler
{
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
}
