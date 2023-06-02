using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerScript : MonoBehaviour
{
    public static DealerScript dealerScript;
    public GameObject CurrentPoint,BoostBuffText,RequestCardBtnText,ErrorText,RequestBtn;

    public int BoostCount = 0, RequestAmount= 0;
    public GameObject[] BoostImg = new GameObject[2];

    public string[] BuffText =  {"부스트 없음","모두가 +3점","모두가 +6점","모두가 X2점"};
    // Start is called before the first frame update

    void Awake()
    {
        dealerScript = this;
       
    }
    void Start()
    {
        ErrorText.GetComponent<Text>().text = "";
        CurrentPoint.GetComponent<Text>().text = "0";
        RequestCardBtnText.GetComponent<Text>().text = "0/3";

        for (int i = 0; i < BoostImg.Length; i++)
        {
            BoostImg[i].GetComponent<Image>().color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BoostBuffText.GetComponent<Text>().text = BuffText[BoostCount];
        RequestCardBtnText.GetComponent<Text>().text = RequestAmount.ToString()+"/"+NetworkManager.networkManager.GetMaxCurrentRoom();
    }

    public void SetMyPoint(int i)
    {
        CurrentPoint.GetComponent<Text>().text = i.ToString();
    }

    public void ShowMeErrorCode(int i)
    {
        ErrorText.GetComponent<Text>().text = "ErrorCode:" + i.ToString();
    }

    public void RequsetOneCard()
    {
        NetworkManager.networkManager.RequsetOneCard();
        RequestBtn.GetComponent<Button>().enabled = false;
    }
    public void BoostCountUP()
    {
        BoostCount++;
        for (int i = 0; i < BoostCount; i++)
        {
            BoostImg[i].GetComponent<Image>().color = new Color(0,0,0,0);
        }
    }
}
