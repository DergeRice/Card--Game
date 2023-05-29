
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public bool HasRob  = false, HasPro = false, HasEx = false, IsME = false;
    public string UserName,MyPoint;

    public int MyNum;
    public GameObject HandCanvas;
    public GameObject ParentObj,Panel;
    public bool IsMine = false;
    public GameObject PlayerText;

    private List<GameObject> MySpecialCardList = new List<GameObject>();

    public GameObject MyPanel;

    // Start is called before the first frame update
    void Start()
    {
        
        UserName =  NetworkManager.networkManager.GetPlayerName(MyNum);
        // if()
        if(IsMine)
        {
            DragAndDrop.dragAndDrop.OrginCanvas = HandCanvas;
            DragAndDrop.dragAndDrop.SetMCToOC(ParentObj);
        }
        else
        {
            PlayerText.SetActive(true);
            PlayerText.GetComponent<Text>().text = UserName+"의 손패입니다.";
        }
        
        MyNum = int.Parse(gameObject.name.Replace("P",""));
    }

    // Update is called once per frame
    void Update()
    {
        CheckMyHandSpecial();
        SetMyStateIndicator();

        if(MyPanel != null)
        {
            //MyPanel.GetComponent<InGamePlayerUI>().NickName.GetComponent<Text>().text = UserName;
            MyPanel.GetComponent<InGamePlayerUI>().PointObj.GetComponent<Text>().text = MyPoint;
        }

        
        
    }
    public void CheckMyHandSpecial()
    {
        HasEx = false; HasPro= false; HasRob = false;

        for (int i = 0; i < HandCanvas.transform.childCount; i++)
        {
            CardScript TempSc;
            HandCanvas.transform.GetChild(i).TryGetComponent<CardScript>(out TempSc);
            if(TempSc != null)
            {
                if (TempSc.SpecialCard == "EX") HasEx = true; 
                if (TempSc.SpecialCard == "PR") HasPro = true; 
                if (TempSc.SpecialCard == "RO") HasRob = true; 
                if(IsMine) TempSc.IsMine = true;
            }
        }
    }
    public void SetMyStateIndicator()
    {
        if(HasEx) MyPanel.GetComponent<InGamePlayerUI>().HasEx = true;
        if(HasPro) MyPanel.GetComponent<InGamePlayerUI>().HasPro = true;
        if(HasRob) MyPanel.GetComponent<InGamePlayerUI>().HasRob = true;
    }

    public void SetMyProfileClickAble()
    {
        if(!HasPro) MyPanel.GetComponent<Button>().enabled = true;
    }

    public void SetMyProfile(int RandNum)
    {
        MyPanel.GetComponent<InGamePlayerUI>().SetMyProfile(RandNum);
    }
}
