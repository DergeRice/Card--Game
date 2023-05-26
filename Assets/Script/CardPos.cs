using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPos : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IDropHandler
{   
    public bool IsInputed;
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

            if(DragingObj.IsSpecial) // 스페셜 카드인지 확인
            {
                if(DragingObj.SpecialCard == "PR") return; //protect는 필드에 놓지마
                if(DragingObj.SpecialCard == "RO"){RobEventOccur();}
                if(DragingObj.SpecialCard == "EX"){ExchangeEventOccur();}
                string CardName = eventData.pointerDrag.GetComponent<CardScript>().SpecialCard;
                int GetCardAmount = eventData.pointerDrag.GetComponent<CardScript>().GetCardAmount;
                InputSpecialCard(CardName,GetCardAmount);
                
                Destroy(eventData.pointerDrag);
            }

            eventData.pointerDrag.transform.parent = transform;
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            //eventData.pointerDrag.transform.localScale = transform.lossyScale;

            if(transform.childCount > 1)
            {
                transform.GetChild(0).parent = NetworkManager.networkManager.MyHand.transform;
            }
        }
        
        // CardManager.cardManager.CheckOver8Card();
    }
    public void RobEventOccur()
    {
        //CardManager.cardManager.
    }
    public void ExchangeEventOccur()
    {

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null) GetComponent<Image>().color = new Color(0,0,0,0.3f);
        else GetComponent<Image>().color = new Color(0,0,0,0f);
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       if(eventData.pointerDrag != null) GetComponent<Image>().color = new Color(0,0,0,0f);
    }
    void Update()
    {
        CheckThisEmpty();
    }
    void CheckThisEmpty()
    {
        if( transform.childCount == 0)
        {
            IsInputed = false;
        }else IsInputed = true;
    }

    void InputSpecialCard(string CardName,int GetCardAmount)
    {
        if (CardName.Contains("EX"))
        {
            // SpecialCard = "EX";
        }
        else if (CardName.Contains("PR"))
        {
            // SpecialCard = "PR";
        }
        else if (CardName.Contains("RO"))
        {
            // SpecialCard = "RO";
        }
        else if (CardName.Contains("GET"))
        {
            // SpecialCard = "GET";
            NetworkManager.networkManager.GiveCardClient(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),NetworkManager.networkManager.GetMyPlayerNum(),GetCardAmount);
            
        }

       
    }
}
