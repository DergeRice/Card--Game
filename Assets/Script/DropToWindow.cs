using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DropToWindow : MonoBehaviour ,IDropHandler
{
    public GameObject Parent;
    public bool MustMyCard, MustOpCard;
   public void OnDrop(PointerEventData eventData)
    {
       
        if(eventData.pointerDrag != null)
        {
            CardScript DragingObj;
            
            
            
            if(eventData.pointerDrag.TryGetComponent<CardScript>(out DragingObj) == false) 
            {
                eventData.pointerDrag.transform.parent =  eventData.pointerDrag.GetComponent<CardScript>().OrginParent.transform;
                return;
            }
            if(MustMyCard) if(!DragingObj.IsMine) return; //내꺼 상대거 확인
            if(MustOpCard) if(DragingObj.IsMine) return;


            if(DragingObj.IsSpecial) // 스페셜 카드인지 확인
            {
                if(DragingObj.SpecialCard == "PR")
                {
                    GetComponent<Image>().color = new Color(0,0,0,0f);
                    return;
                }  //protect는 필드에 놓지마
                if(DragingObj.SpecialCard == "EX")
                {
                    GetComponent<Image>().color = new Color(0,0,0,0f);
                    return;
                }
            }
            if(DragingObj.OrginParent != transform) Parent = DragingObj.OrginParent;
            

            eventData.pointerDrag.transform.parent = transform;
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            if(transform.childCount > 1)
            {
                transform.GetChild(0).parent = DragingObj.GetComponent<CardScript>().OrginParent.transform;
            }
        }
    }
}
