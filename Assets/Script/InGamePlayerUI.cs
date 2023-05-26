using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour
{
    public bool IsFirst = false, HasRob  = false, HasPro = false, HasEx = false, IsME = false;
    public GameObject Bg,Crown,Rob,Ex,Pro,CharImg,NickName,MeIndicator,PointObj;
    public List<Sprite> CharImgs = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        //CharImg.GetComponent<Image>().sprite = CharImgs[Random.Range(0,CharImgs.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFirst) Crown.SetActive(true);
        else Crown.SetActive(false);

        if(HasRob) Rob.SetActive(true);
        else Rob.SetActive(false);

        if(HasPro) Pro.SetActive(true);
        else Pro.SetActive(false);

        if(HasEx) Ex.SetActive(true);
        else Ex.SetActive(false);

        if(IsME) MeIndicator.SetActive(true);
        else MeIndicator.SetActive(false);
    }

    public void SetMyProfile(int RandNum)
    {
        RandNum %= CharImgs.Count;
        CharImg.GetComponent<Image>().sprite = CharImgs[Random.Range(0,CharImgs.Count)];
    }
}
