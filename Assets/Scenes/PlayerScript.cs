
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public bool HasRob  = false, HasPro = false, HasEx = false, IsME = false;
    public string UserName,MyPoint;
    public GameObject HandCanvas;
    public GameObject ParentObj;
    public bool IsMine = false;


    private List<GameObject> MySpecialCardList = new List<GameObject>();

    public GameObject MyPanel;

    // Start is called before the first frame update
    void Start()
    {
        if(IsMine)
        {
            DragAndDrop.dragAndDrop.OrginCanvas = HandCanvas;
            DragAndDrop.dragAndDrop.SetMCToOC(ParentObj);
        }
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
        for (int i = 0; i < HandCanvas.transform.childCount; i++)
        {
            CardScript TempSc = HandCanvas.transform.GetChild(i).GetComponent<CardScript>();
            if(TempSc.IsSpecial)
            {
                if (TempSc.SpecialCard == "EX") HasEx = true; 
                else HasEx = false;
                if (TempSc.SpecialCard == "PR") HasPro = true; 
                else HasPro = false;
                if (TempSc.SpecialCard == "RO") HasRob = true; 
                else HasRob = false;
            }
        }
    }
    public void SetMyStateIndicator()
    {
        if(HasEx) MyPanel.GetComponent<InGamePlayerUI>().HasEx = true;
        if(HasPro) MyPanel.GetComponent<InGamePlayerUI>().HasPro = true;
        if(HasRob) MyPanel.GetComponent<InGamePlayerUI>().HasRob = true;
    }

    public void SetMyProfile(int RandNum)
    {
        MyPanel.GetComponent<InGamePlayerUI>().SetMyProfile(RandNum);
    }
}
