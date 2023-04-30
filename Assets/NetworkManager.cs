using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static string PlayerNAME;
    public static string RoomNAME;
    public static List<RoomInfo> Mylist = new List<RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LogInGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Login Success");
    }

    public static void JoinLobby()
    {
        
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LocalPlayer.NickName = PlayerNAME;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }

    public static void CreateRoomList()
    {
        PhotonNetwork.CreateRoom(RoomNAME,new RoomOptions { MaxPlayers = 4 });
    }
    public static void JoinRoomList(string RoomName)
    {
       
        
        Debug.Log($"JoinedRoom: {RoomName}");
        PhotonNetwork.JoinRoom(RoomName);
        //RoomMaxRefresh(PhotonNetwork.CurrentRoom);
    }


    public override void OnCreatedRoom()
    {
         for (int i = 0; i < Mylist.Count; i++)
        {
           Debug.Log("dd");
        }

        
        Debug.Log($"CreatedRoom: {RoomNAME}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if(!roomList[i].RemovedFromList){
                if(Mylist.Contains(roomList[i])) Mylist.Add(roomList[i]);
                else Mylist[Mylist.IndexOf(roomList[i])] = roomList[i];
            }
            else if(Mylist.IndexOf(roomList[i]) != -1) Mylist.RemoveAt(Mylist.IndexOf(roomList[i]));
        }

        Debug.Log("Dd");
    }

    
    public static void RoomMaxRefresh(Room room)
    {
        
    }

    public static string SetMyMaxPlayer(string RoomName)
    {
        for (int i = 0; i < Mylist.Count; i++)
        {
            if(Mylist[i].Name == RoomName){
                return Mylist[i].PlayerCount + "/4";
            }
        }
         return "";
    }

    
}
