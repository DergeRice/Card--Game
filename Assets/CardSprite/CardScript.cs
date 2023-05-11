using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public string CardType;

    public string PartOfSpeech;
    public char Operator;
    public int PlusCount;

    public bool IsSpecial;

    public string SpecialCard;

    public int GetCardAmount;

    public bool IsUsed;

    [SerializeField]
    GameObject CardImg;
    string CardName;

    
    // Start is called before the first frame update
    void Start()
    {
        CardName = CardImg.GetComponent<Image>().sprite.name;
        GetCardType();
        GetOperator();
        GetSpecial();
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    #region  CardType
    void GetCardType()
    {
        if (CardName.Contains("대명사"))
        {
            CardType = "대명사";
            PartOfSpeech = "명사";
        }
        else if (CardName.Contains("셀없명사"))
        {
            CardType = "셀없명사";
            PartOfSpeech = "명사";
        }
        else if (CardName.Contains("소유격"))
        {
            CardType = "소유격";
        }
        else if (CardName.Contains("부정관사"))
        {
            CardType = "부정관사";
            PartOfSpeech = "관사";
        }
        else if (CardName.Contains("셀있명사"))
        {
            CardType = "셀있명사";
            PartOfSpeech = "명사";
        }
        else if (CardName.Contains("정관사"))
        {
            CardType = "정관사";
            PartOfSpeech = "관사";
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

        if(SpecialCard != null) IsSpecial = true;
    }
}
