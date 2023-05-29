using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager cardManager;
    public int CardInputCount;

    public GameObject[] CardPos = new GameObject[16];
    //public CardScript[] InputedCardInfo = new CardScript[16];

    public List<CardScript> InputedCardInfo = new List<CardScript>();

    int MyPoint = 0;

    public GameObject RobbParent;

    private void Start() 
    {
        NetworkManager.networkManager.SetInitialFun();
        // for (int i = 0; i < CardPos.Length; i++)
        // {
        //     CardPos[i] = transform.GetChild(i).gameObject;
        // }
        cardManager = this;
    }

    public void CheckOver8Card()
    {
        CardInputCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(CardPos[i].GetComponent<CardPos>().IsInputed) CardInputCount++;
        }

        if(CardInputCount > 6)
        {
            EnableAllChild();
        }
    }

    private void Update() 
    {
        if(transform.childCount > 1) CheckOver8Card();    
    }

    public void EnableAllChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CardPos[i].SetActive(true);
        }
    }

    [ContextMenu("GiveMeCard")]
    public void GiveMeCard()
    {
        NetworkManager.networkManager.GiveCardClient(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),NetworkManager.networkManager.GetMyPlayerNum(),1);
    }

    [ContextMenu("GiveMeAllSpecial")]
    public void GiveMeAllSpecial()
    {
        GameObject temp = GameObject.Find("CardDeck");
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            if(temp.transform.GetChild(i).GetComponent<CardScript>().IsSpecial)
            {
                NetworkManager.networkManager.GiveCardClient(i,NetworkManager.networkManager.GetMyPlayerNum(),1);
            }
        }
    }
    [ContextMenu("Give1AllSpecial")]
    public void Give1AllSpecial()
    {
        GameObject temp = GameObject.Find("CardDeck");
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            if(temp.transform.GetChild(i).GetComponent<CardScript>().IsSpecial)
            {
                NetworkManager.networkManager.GiveCardClient(i,1,1);
            }
        }
    }
    [ContextMenu("GiveALLOneCard")]
    public void GiveAllOneCard()
    {
        for (int i = 0; i < NetworkManager.networkManager.PlayerCanvas.Count; i++)
        {
           NetworkManager.networkManager.GiveCardClient(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),i,1); 
        }
    }
    [ContextMenu("GiveALLThreeCard")]
    public void GiveAllThreeCard()
    {
        for (int i = 0; i < NetworkManager.networkManager.PlayerCanvas.Count; i++)
        {
           NetworkManager.networkManager.GiveCardClient(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),i,3); 
        }
    }


    [ContextMenu("Cal")]
    public void CalMySenctence()
    {
        if(CheckMySentenceAvailable()==0) MyPoint = CalMyPoint();
        Debug.Log(CheckMySentenceAvailable()+"/"+MyPoint);
    }

    public int CheckMySentenceAvailable()
    {
        InputedCardInfo = new List<CardScript>();
        bool PointAble = false, IsVerbNormal = false;

        List<int> VerbPos = new List<int>();
        List<int> NounPos = new List<int>();
        List<bool> CheckList  = new List<bool>();

        for (int i = 0; i < CardPos.Length; i++)
        {
            if(CardPos[i].transform.childCount > 0)
            {
                InputedCardInfo.Add(CardPos[i].transform.GetChild(0).GetComponent<CardScript>());
            }   
        }

        for (int i = 0; i < InputedCardInfo.Count; i++)
        {
            if (InputedCardInfo[i].PartOfSpeech == "동사") //가장먼저 동사부터 찾아야함
            {
                if(i == 0) return 1001;
                if(InputedCardInfo[i-1].PartOfSpeech == "명사" && InputedCardInfo[i-1].IsRealNoun) return 1002; //바로 앞에 명사고 진짜 명사인지 확인
                if(InputedCardInfo[i].CardType == "일반동사") IsVerbNormal = true;
                else IsVerbNormal = false;

                VerbPos.Add(i);
            }
        }

        for (int i = 0; i < InputedCardInfo.Count; i++) // 나머지들 품사 구분
        {
            InputedCardInfo[i].MakeRealNoun(); // 뒤에 명사 셀 수 있게 만들기
            // InputedCardInfo[i].MakeAdjectiveAble(); // 뒤에 명사에 형용사 붙이기
            
            if(InputedCardInfo[i].PartOfSpeech == "형용사")
            {
                if(i == 0) return 1003;
                if(!(InputedCardInfo[i+1].PartOfSpeech == "명사" || InputedCardInfo[i-1].CardType == "비동사")) return 1004; //형용사 바로 뒤에 명사거나 앞에 동사면 아니면 문장 안됨
            }

            if(InputedCardInfo[i].CardType == "빈도부사" )
            {
                if(i == 0) return 1005;
                if(!(InputedCardInfo[i-1].CardType == "일반동사" || InputedCardInfo[i+1].CardType == "비동사")) return 1006; //형용사 바로 뒤에 명사거나 앞에 동사면 아니면 문장 안됨
            }

            if(InputedCardInfo[i].CardType == "두")
            {
                if(!IsVerbNormal) return 1007;
                bool check = false;
                if(i == 0)// Do 가 의문문일때는 뒤에 명사가 와야한다.
                {
                    for (int j = i; j < InputedCardInfo.Count; i++)
                    {   
                        
                        if(!InputedCardInfo[j].IsNounFamily) return 1008;
                        if(InputedCardInfo[j].PartOfSpeech == "명사")
                        {
                            check = true;
                            break;
                        } 
                    }
                }
                else // 아닌경우에는 부정문이므로 낫이 와야한다. 
                {
                    for (int j = i; j < InputedCardInfo.Count; i++)
                    {   
                        if(InputedCardInfo[j].CardType == "낫")
                        {
                            check = true;
                            break;
                        } 
                    }
                }
                PointAble = check && PointAble;
            }

            if(InputedCardInfo[i].CardType == "조동사")
            {
                bool check = false;
                if(i == 0)// 조동사 가 의문문일때는 뒤에 명사가 와야한다.
                {
                    for (int j = i; j < InputedCardInfo.Count; i++)
                    {   
                        
                        if(!InputedCardInfo[j].IsNounFamily) return 1009;
                        if(InputedCardInfo[j].PartOfSpeech == "명사")
                        {
                            check = true;
                            break;
                        } 
                    }
                }
                PointAble = check && PointAble;
            }
            if(InputedCardInfo[i].CardType == "낫")
            {
                bool check = false;
                if(i == 0) return 1010;
                if(IsVerbNormal) //일반동사일 경우에는 낫이 Do나 조동사가 와야한다.
                {
                    for (int j = i; j > 0; i--)
                    {
                        if(!InputedCardInfo[j].IsNounFamily) return 1011;
                        if(InputedCardInfo[j].CardType == "두" || InputedCardInfo[j].CardType == "조동사")
                        {
                            check = true;
                            break;
                        } 
                    }
                }
                else // Be동사일때는 바로 앞이 비동사여야한다.
                {
                    if(InputedCardInfo[i-1].PartOfSpeech != "동사") return 1012; //앞에가 비동사 아니면 false;
                }
                PointAble = check && PointAble;
            }


            if(InputedCardInfo[i].CardType == "전치사" && i > 1)
            {
                bool check = false;
                if(i == 0) return 1013;
                for (int j = i; j < InputedCardInfo.Count; i++)
                {
                    if(!InputedCardInfo[j].IsNounFamily) return 1014;
                    if(InputedCardInfo[j].PartOfSpeech == "명사")
                    {
                        check = true;
                        break;
                    } 
                }

                PointAble = check && PointAble;
            }

            if(InputedCardInfo[i].CardType == "소유격" && i > 1)
            {
                bool check = false;
                for (int j = i; j < InputedCardInfo.Count; i++)
                {
                    if(!InputedCardInfo[j].IsNounFamily) break;
                    if(InputedCardInfo[j].PartOfSpeech == "명사")
                    {
                        InputedCardInfo[j].IsRealNoun = true;
                        check = true;
                        break;
                    } 
                }
                PointAble = check && PointAble;
            }


            if (InputedCardInfo[i].PartOfSpeech == "명사")
            {
                if(InputedCardInfo[i].CardType == "셀없명사") 
                {
                    if(InputedCardInfo[i].IsRealNoun) // 셀없명사면 진짜 명사 되는지 확인하고 값넣어줌
                    {
                        NounPos.Add(i);
                    }
                }else // 대명사나 명사는 그냥 보내주고
                {
                    NounPos.Add(i);
                }
            }
        }
       
        if(VerbPos.Count < 1 || NounPos.Count < 1) return 1015; // 둘중 하나라도 한개조차 없으면 false return;
        else //둘다 하나씩 있으면 본격적인 검사 시작
        {
            //PointAble = true;
            // PointAble |= Check3Format(IsVerbNormal,NounPos,VerbPos);
            // PointAble |= Check4Format(IsVerbNormal,NounPos,VerbPos);
            // PointAble |= Check5Format(IsVerbNormal,NounPos,VerbPos);

            return 0;
        }
    }
    bool Check3Format(bool IsVerbNormal, List<int> NounPos,List<int> VerbPos)
    {
        if(IsVerbNormal == false) return false;
        return true;
    }
    bool Check4Format(bool IsVerbNormal, List<int> NounPos,List<int> VerbPos)
    {
        if(IsVerbNormal == false) return false;
        if(NounPos.Count < 3) return false;
        return true;
    }
    bool Check5Format(bool IsVerbNormal, List<int> NounPos,List<int> VerbPos)
    {
        if(IsVerbNormal == false) return false;
        return true;
    }


    public int CalMyPoint()
    {
        int FinalPoint = CardInputCount;

        for (int i = 0; i < InputedCardInfo.Count; i++)
        {
            if(InputedCardInfo[i].PlusCount != 0)
            {
                if(InputedCardInfo[i].Operator == '*') FinalPoint *= InputedCardInfo[i].PlusCount;
                if(InputedCardInfo[i].Operator == '/') FinalPoint /= InputedCardInfo[i].PlusCount;
            }
        }

        for (int i = 0; i < InputedCardInfo.Count; i++)
        {
            if(InputedCardInfo[i].PlusCount != 0)
            {
                if(InputedCardInfo[i].Operator == '+') FinalPoint += InputedCardInfo[i].PlusCount;
                if(InputedCardInfo[i].Operator == '-') FinalPoint -= InputedCardInfo[i].PlusCount;
            }
        }

        return FinalPoint;
    }

    public void RobEnd()
    {
        if(RobbParent.transform.childCount == 0 ) return;
        GameObject RobedCard = RobbParent.transform.GetChild(0).gameObject;
        NetworkManager.networkManager.RobbedCard = RobedCard;
        NetworkManager.networkManager.EventTargetNum = RobbParent.GetComponent<DropToWindow>().Parent.GetComponent<MyHandScript>().MyPlayerNum;
        NetworkManager.networkManager.RobConfirm();
    }
}
