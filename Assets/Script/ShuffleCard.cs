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

    [SerializeField] GameObject PlayerObj;
    //List<GamePlayer> Players = new List<GamePlayer>();// Player Player1;

    List<GameObject> PlayersHand = new List<GameObject>();

    NetworkManager network;

    PhotonView PV;

    GameObject MyDeck;

    

    [ContextMenu("card gogo")]
    public void Initial_Card()
    {
        NetworkManager.networkManager.PlayersHand = PlayersHand;
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < PlayersHand.Count; i++)
            {
                NetworkManager.networkManager.GiveCard(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),i);
            }
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
            if(p.NickName == PhotonNetwork.NickName)
            {
                
                NetworkManager.networkManager.MyHand = PlayerHand.GetComponent<PlayerScript>().HandCanvas;
                PlayerHand.GetComponent<PlayerScript>().IsMine = true;
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
