using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class ShuffleCard : MonoBehaviour
{

    [SerializeField]
    GameObject Hand;
    List<GamePlayer> Players = new List<GamePlayer>();// Player Player1;

    NetworkManager network;

    PhotonView PV;

    

    [ContextMenu("card gogo")]
    public void Initial_Card()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GiveCard(Players[i]);
            }
        }
        
    }
    
    private void StartGame() 
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GamePlayer gamePlayer = new GamePlayer();
            gamePlayer.MakePlayer("P"+p.ActorNumber.ToString()); 
            gamePlayer.Hand = Hand;
            Players.Add(gamePlayer);

            GameObject playerObject = PhotonView.Find(p.ActorNumber).gameObject;
            PhotonView playerPhotonView = playerObject.GetComponent<PhotonView>();
            if (playerPhotonView != null)
            {
                gamePlayer.ThisPlayerView = playerPhotonView;
            }
            Debug.Log(gamePlayer.PlayerName+gamePlayer.ThisPlayerView.ToString()+"DDDD");
        }
        PV = GetComponent<PhotonView>();
    }

    private void Start() 
    {
        network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartGame();
        GameObject.Find("GameManager").GetComponent<CardCreator>().MakeCardDeck();
    }

    public void GiveCard(GamePlayer P) //Give A to One Card
    {
        // if(P.ThisPlayerView.Owner == PhotonNetwork.MasterClient)
        // {
        //     P.GetCard();
        // }
        // else 
        PV.RPC("GetCard", P.ThisPlayerView.Owner);
    }
}

public class GamePlayer : MonoBehaviour 
{
    public string PlayerName;
    int Point;
    int CardCount;

    bool HavingRobCard;
    bool HavingEXCard;
    bool HavingPROTECTCard;

    public GameObject Hand;

    public PhotonView ThisPlayerView;
    List<GameObject> HavingCard = new List<GameObject>();

    [PunRPC]
    public void GetCard()
    {
        this.CardCount++;
        Instantiate(NetworkManager.cardDeck.transform.GetChild(0).gameObject,Hand.transform);
        Destroy(NetworkManager.cardDeck.transform.GetChild(0).gameObject);
        Debug.Log("tlqkffhadk");
    }
    public void MakePlayer(string A)
    {
        this.PlayerName = A; 
    }
} 
