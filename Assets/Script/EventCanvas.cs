using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCanvas : MonoBehaviour
{
    public static EventCanvas eventCanvas;
    public GameObject RobOwner, RobSub, ExOwner,ExSub, RobbedCardObj;

    public GameObject OwnMyCardExchange, OwnOpCardExchange,SubMyCardExchange, SubOpCardExchange,ExchangeReceive,ExchangeDenyPanel;

    public GameObject SuccessText, DenyText, ReceiveMsg,SendingMsg;
    // Start is called before the first frame update
    void Start()
    {
        eventCanvas = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RobConfirmBtn()
    {
        RobSub.SetActive(false);
    }

    public void RobEventSub(string CardName)
    {
        Sprite IndicateImg = GameObject.Find(CardName).transform.GetChild(0).GetComponent<Image>().sprite;
        RobbedCardObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = IndicateImg;
    }

    public void ExEventSub(string MyCard,string OpCard)
    {
        Sprite IndicateMyCardImg = GameObject.Find(MyCard).transform.GetChild(0).GetComponent<Image>().sprite;
        Sprite IndicateOpCardImg = GameObject.Find(OpCard).transform.GetChild(0).GetComponent<Image>().sprite;

        SubMyCardExchange.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = IndicateMyCardImg;
        SubOpCardExchange.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = IndicateOpCardImg;
    }
    public void DenyExchange()
    {
        ExchangeDenyPanel.SetActive(true);
    }
    public void ConfirmExchange()
    {
        NetworkManager.networkManager.ExSubEndResult(true,SendingMsg.GetComponent<Text>().text);
        ExSub.SetActive(false);
    }
    public void SendExchangeMsg()
    {
        NetworkManager.networkManager.ExSubEndResult(false,SendingMsg.GetComponent<Text>().text);
        ExSub.SetActive(false);
    }
    public void ExchangeRequest()
    {
        string MyExCard = OwnMyCardExchange.transform.GetChild(0).gameObject.name;
        string OpExCard = OwnOpCardExchange.transform.GetChild(0).gameObject.name;
        int OpOwnerNum = OwnOpCardExchange.transform.GetChild(0).gameObject.GetComponent<CardScript>().MyOwnerNum-1;
        NetworkManager.networkManager.ExchangeRequest(OpOwnerNum,MyExCard,OpExCard);
    }
    public void ReceivedDenyMsg(string Msg)
    {
        SuccessText.SetActive(false);
        DenyText.SetActive(true);
        ReceiveMsg.GetComponent<Text>().text = Msg;
    }
    public void ConfirmReciveMsg()
    {
        ExchangeReceive.SetActive(false);
    }
}
