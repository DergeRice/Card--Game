using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleCard : MonoBehaviour
{
    List<GamePlayer> Players = new List<GamePlayer>();// Player Player1;
    public void GiveCard(GamePlayer P) //Give A to One Card
    {
        P.GetCard();
    }

    public void Initial_Card()
    {
        
        for (int i = 0; i < 5; i++)
        {
            GamePlayer P = new GamePlayer();
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

public class GamePlayer
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
