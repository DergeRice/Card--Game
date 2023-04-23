using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleCard : MonoBehaviour
{
    List<Player> Players = new List<Player>();// Player Player1;
    public void GiveCard(Player P) //Give A to One Card
    {
        P.GetCard();
    }

    public void Initial_Card()
    {
        
        for (int i = 0; i < 5; i++)
        {
            Player P = new Player();
            P.MakePlayer("P"+(i).ToString()); 
            Players.Add(P);
        }

        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GiveCard(Players[i]);
            }
        }
        
    }
    [ContextMenu("Start Game")]
    private void Start() 
    {
        Initial_Card();
    }
}

public class Player
{
    string PlayerName;
    int Point;
    int CardCount;

    bool HavingRobCard;
    bool HavingEXCard;
    bool HavingPROTECTCard;
    List<GameObject> HavingCard = new List<GameObject>();

    public void GetCard()
    {
        this.CardCount++;
        Debug.Log(this.PlayerName.ToString()+"/"+this.CardCount.ToString());
    }
    public void MakePlayer(string A)
    {
        this.PlayerName = A; 
    }
} 
