using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class CardCreator : MonoBehaviour
{
    [SerializeField]
    Sprite[] CardImg = new Sprite[58];

    [SerializeField]
    GameObject CardPrefeb,CardParent,CardDeck,CardCreatorObj;
    [SerializeField]
    PhotonView PV;

    NetworkManager Network;

    private void Awake() 
    {
       Network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
       Network.CardDeck = CardCreatorObj;
    }

    public GameObject MakeCard()
    {
        GameObject MadeCard = Instantiate(CardPrefeb,CardParent.transform);
        MadeCard.transform.GetChild(0).GetComponent<Image>().sprite = CardImg[UnityEngine.Random.Range(0,CardImg.Length)];

        return MadeCard;
    }

    [ContextMenu("dd")]
    public List<GameObject> MakeCardDeck()
    {
        List<Sprite> shuffledCards = CardImg.OrderBy(x => Guid.NewGuid()).ToList();

        List<GameObject> MadeCardDeck = new List<GameObject>();

        
        for (int i = 0; i < CardImg.Length; i++)
        {
            GameObject MadeCard = Instantiate(CardPrefeb,CardCreatorObj.transform);
            MadeCard.name = "Card"+i.ToString();
            MadeCard.transform.GetChild(0).GetComponent<Image>().sprite = shuffledCards[i];
            MadeCardDeck.Add(MadeCard);
        }
        
        
        return MadeCardDeck;
    }
}
