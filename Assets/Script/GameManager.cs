using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public List<GameObject> PlayerRankObj = new List<GameObject>();
    public List<int> PlayerPoint = new List<int>();

    private void Start()
    {
        gameManager = this;
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("PlayerCanvas");
        
        foreach (GameObject playerObj in playerObjs)
        {
            PlayerRankObj.Add(playerObj);
            PlayerPoint.Add(playerObj.GetComponent<PlayerScript>().MyPoint);
        }
    }

    private void Update()
    {
        for (int i = 0; i < PlayerRankObj.Count; i++)
        {
            PlayerPoint[i] = PlayerRankObj[i].GetComponent<PlayerScript>().MyPoint;
        }
        
        if (AllPlayerPointsAreZero())
        {
            return;
        }
        SortByPlayerPoint();
        IndicateFirst();
        
    }
    
    private bool AllPlayerPointsAreZero()
    {
        foreach (int point in PlayerPoint)
        {
            if (point != 0)
            {
                return false;
            }
        }

        return true;
    }

    private void SortByPlayerPoint()
    {
        PlayerRankObj.Sort((a, b) => b.GetComponent<PlayerScript>().MyPoint.CompareTo(a.GetComponent<PlayerScript>().MyPoint));
    }


    public void IndicateFirst()
    {
        PlayerRankObj[0].GetComponent<PlayerScript>().IGotFirst();

        for (int i = 1; i < PlayerRankObj.Count; i++)
        {
            PlayerRankObj[i].GetComponent<PlayerScript>().ILostFirst();
        }
    }
}
