using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerExitHandler, IPointerUpHandler, IPointerEnterHandler
{
    public string CardType;

    public string PartOfSpeech;
    public char Operator;
    public int PlusCount;

    public bool IsSpecial;

    public string SpecialCard;

    public int GetCardAmount;

    public bool IsUsed;
    public bool IsOnField;
    public bool IsRealNoun;

    public bool IsNounFamily;

    [SerializeField]
    GameObject CardImg;
    string CardName;

    [SerializeField] GameObject OrginParent;
    


    public bool isDragging = false;
    private RectTransform rectTransform;
    private Vector2 offset;

    public Vector2 ScaleOffset;

    
    // Start is called before the first frame update
    void OnEnable()
    {
        IsSpecial = false;
        ScaleOffset = new Vector2(1,0.7f);
        CardName = CardImg.GetComponent<Image>().sprite.name;
        GetCardType();
        GetOperator();
        GetSpecial();

        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent.tag == "CardPos") IsOnField = true;
        else IsOnField = false;
    }

    public void MakeRealNoun()
    {
        if(!IsOnField) return;
        if(CardType == "소유격" || PartOfSpeech == "관사")
        {
            if(IsUsed == true) return;
            for (int i = GetMyOrder(); i < transform.parent.childCount; i++)
            {
                CardScript Temp;
                Temp = transform.parent.parent.GetChild(i).GetChild(0).GetComponent<CardScript>();

                if(Temp.CardType == "셀없명사")
                {
                    Temp.IsRealNoun = true;
                    IsUsed = true;
                }
            }
        }
    }
    public void MakeAdjectiveAble()
    {
        if(CardType == "형용사")
        {

        }
    }

    int GetMyOrder()
    {
        for (int i = 0; i < transform.parent.parent.childCount; i++)
        {
            if(transform.parent.parent.GetChild(i).gameObject == gameObject) return i ;
        }

        return -1;
    }
    #region  CardType
    void GetCardType()
    {
        if (CardName.Contains("대명사"))
        {
            CardType = "대명사";
            PartOfSpeech = "명사";
            IsNounFamily = true;
        }
        else if (CardName.Contains("셀없명사"))
        {
            CardType = "셀없명사";
            PartOfSpeech = "명사";
            IsNounFamily = true;
        }
        else if (CardName.Contains("소유격"))
        {
            CardType = "소유격";
            IsNounFamily = true;
        }
        else if (CardName.Contains("부정관사"))
        {
            CardType = "부정관사";
            PartOfSpeech = "관사";
            IsNounFamily = true;
        }
        else if (CardName.Contains("셀있명사"))
        {
            CardType = "셀있명사";
            PartOfSpeech = "명사";
            IsNounFamily = true;
        }
        else if (CardName.Contains("정관사"))
        {
            CardType = "정관사";
            PartOfSpeech = "관사";
            IsNounFamily = true;
        }
        else if (CardName.Contains("비동사"))
        {
            CardType = "비동사";
            PartOfSpeech = "동사";
        }
        else if (CardName.Contains("일반동사"))
        {
            CardType = "일반동사";
            PartOfSpeech = "동사";
        }
        else if (CardName.Contains("전치사"))
        {
            CardType = "전치사";
            PartOfSpeech = "전치사";
        }
        else if (CardName.Contains("낫"))
        {
            CardType = "낫";
        }
        else if (CardName.Contains("베리"))
        {
            CardType = "베리";
        }
        else if (CardName.Contains("부사"))
        {
            CardType = "부사";
            PartOfSpeech = "부사";
        }
        else if (CardName.Contains("빈도부사"))
        {
            CardType = "빈도부사";
        }
        else if (CardName.Contains("조동사"))
        {
            CardType = "조동사";
        }
        else if (CardName.Contains("형용사"))
        {
            CardType = "형용사";
            PartOfSpeech = "형용사";
            IsNounFamily = true;
        }
        else if (CardName.Contains("두"))
        {
            CardType = "두";
            PartOfSpeech = "조동사";
        }
        
    }
    #endregion
    
    void GetOperator()
    {
        if (CardName.Contains("*"))
        {
            Operator = '*';
            PlusCount = int.Parse(CardName.Substring(CardName.IndexOf("*")+1, 1));
        }
        else if (CardName.Contains("_"))
        {
            Operator = '/';
            PlusCount = int.Parse(CardName.Substring(CardName.IndexOf("_")+1, 1));
        }
        else if (CardName.Contains("+"))
        {
            Operator = '+';
            PlusCount = int.Parse(CardName.Substring(CardName.IndexOf("+")+1, 1));
        }
        else if (CardName.Contains("-"))
        {
            Operator = '-';
            PlusCount = int.Parse(CardName.Substring(CardName.IndexOf("-")+1, 1));
        }
    }

    void GetSpecial()
    {
        if (CardName.Contains("EX"))
        {
            SpecialCard = "EX";
        }
        else if (CardName.Contains("PR"))
        {
            SpecialCard = "PR";
        }
        else if (CardName.Contains("RO"))
        {
            SpecialCard = "RO";
        }
        else if (CardName.Contains("GET"))
        {
            SpecialCard = "GET";
            GetCardAmount = int.Parse(CardName.Substring(CardName.IndexOf("T")+1, 1));
        }

        if(SpecialCard != "") IsSpecial = true;
    
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OrginParent = transform.parent.gameObject;
        transform.parent = DragAndDrop.dragAndDrop.MovingCanvas.transform;
        offset = rectTransform.anchoredPosition - eventData.position;
        rectTransform.anchoredPosition = (eventData.position * ScaleOffset) + offset;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            rectTransform.anchoredPosition = (eventData.position * ScaleOffset) + offset;
            GetComponent<CanvasGroup>().alpha = 0.6f;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(transform.parent.gameObject.name == "MovingCanvas")
        {
            transform.parent = OrginParent.transform;
        }
        isDragging = false;
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void PlayDrawAnimation()
    {
        GameObject Dealer = GameObject.Find("Dealer").transform.GetChild(0).GetChild(3).gameObject;
        Dealer.SetActive(true);
        Dealer.GetComponent<Image>().sprite = CardImg.GetComponent<Image>().sprite;
        Dealer.GetComponent<Animator>().Play("Throw");
        StartCoroutine(DisableAfterAni(Dealer));
    }

    IEnumerator DisableAfterAni(GameObject Dis)
    {
        yield return new WaitForSeconds(0.4f);
        Dis.SetActive(false);
    }
}