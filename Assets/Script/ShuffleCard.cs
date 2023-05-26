using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class ShuffleCard : MonoBehaviour
{

    [SerializeField]
    GameObject Hand;

    [SerializeField] GameObject PlayerObj;
    //List<GamePlayer> Players = new List<GamePlayer>();// Player Player1;

    List<GameObject> PlayersHand = new List<GameObject>();

    public List<GameObject> PlayerPanel = new List<GameObject>();

    NetworkManager network;

    PhotonView PV;

    GameObject MyDeck;

    

    [ContextMenu("card gogo")]
    public void Initial_Card()
    {
        NetworkManager.networkManager.PlayersHand = PlayersHand;
        for (int i = 0; i < PlayersHand.Count; i++)
        {
            NetworkManager.networkManager.GiveCard(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),i,5);
        }
    
        
    }
    private void StartGame() 
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            
            GameObject PlayerHand;
            PlayerHand = Instantiate(PlayerObj);
            
            PlayersHand.Add(PlayerHand);
            PlayerHand.name = "P"+p.ActorNumber.ToString();

            PlayerPanel[p.ActorNumber-1].SetActive(true); //패널활성화
            PlayerPanel[p.ActorNumber-1].GetComponent<InGamePlayerUI>().NickName.GetComponent<Text>().text = p.NickName;
            Debug.Log(p.NickName);
            PlayerHand.GetComponent<PlayerScript>().MyPanel = PlayerPanel[p.ActorNumber-1];
            NetworkManager.networkManager.SetInitialFun(p.ActorNumber);
            if(p.NickName == PhotonNetwork.NickName) // 내꺼라는소리
            {
                NetworkManager.networkManager.MyHand = PlayerHand.GetComponent<PlayerScript>().HandCanvas;
                PlayerHand.GetComponent<PlayerScript>().IsMine = true;
                PlayerPanel[p.ActorNumber-1].GetComponent<InGamePlayerUI>().IsME = true;
            }
            else PlayerHand.transform.position = new Vector3(100,100,100);
            
        }
        Initial_Card();
    }

    private void Start() 
    {
        PV = GetComponent<PhotonView>();
        MyDeck = PhotonNetwork.Instantiate("CardDeck",Vector3.zero,Quaternion.identity);
        MyDeck.name = "CardDeck";
        StartGame();
    }
}
