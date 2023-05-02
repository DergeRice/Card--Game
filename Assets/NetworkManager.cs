using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static string PlayerNAME;
    public static string RoomNAME;
    public static List<RoomInfo> Mylist = new List<RoomInfo>();

    public static GameObject RoomParent { get; set; }
    public static GameObject RoomPrefeb { get; set; }

    public static GameObject PlayerParent { get; set; }
    public static GameObject PlayerPrefeb { get; set; }



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

    public static void CreateRoomList(string RoomName)
    {
        PhotonNetwork.CreateRoom(RoomName,new RoomOptions { MaxPlayers = 4 });
        Debug.Log(RoomName);
        //
    }

    public static void JoinRoomList(string RoomName)
    {
        Debug.Log($"JoinedRoom: {RoomName}");
        PhotonNetwork.JoinRoom(RoomName);
        RoomNAME = RoomName;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       ShowMePlayerList();
    }

    
    public static void RefreshRoom()
    {
        Debug.Log(PhotonNetwork.PlayerList);
    }


    public override void OnCreatedRoom()
    {
       // Debug.Log($"CreatedRoom: {RoomNAME}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if(!roomList[i].RemovedFromList){
                if(!Mylist.Contains(roomList[i])) Mylist.Add(roomList[i]);
                else Mylist[Mylist.IndexOf(roomList[i])] = roomList[i];
            }
            else if(Mylist.IndexOf(roomList[i]) != -1) Mylist.RemoveAt(Mylist.IndexOf(roomList[i]));

            
            ShowMeRoomList();
        }

        
    }
    public static int GetRoomNum(string RoomName)
    {
        for (int i = 0; i < Mylist.Count; i++)
        {
            if(Mylist[i].Name == RoomName) return i;
        }
        
        return -1;
    }

    public static void ShowMeRoomList()
    {
        if(RoomParent.transform.childCount != 0)
        {
            for (int i = 0; i < RoomParent.transform.childCount; i++)
            {
                Destroy(RoomParent.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < PhotonNetwork.CountOfRooms; i++)
        {
            GameObject Temp = Instantiate(RoomPrefeb,RoomParent.transform);
            Temp.transform.Find("RoomName").gameObject.GetComponent<Text>().text = Mylist[i].Name;
            //Debug.Log(Mylist[GetRoomNum(RoomName)]);
            Temp.transform.Find("RoomMax").gameObject.GetComponent<Text>().text = $"{Mylist[i].PlayerCount}/4";
        }
        
    }

    
    public static void ShowMePlayerList()
    {
        if(PlayerParent.transform.childCount != 0)
        {
            for (int i = 0; i < PlayerParent.transform.childCount; i++)
            {
                Destroy(PlayerParent.transform.GetChild(i).gameObject);
            }
        }
        RoomInfo ThisRoomInfo = Mylist[GetRoomNum(RoomNAME)];

        List<Player> ThisRoomPlayerlist = new List<Player>();//<Player>();
        //Player[] ThisRoomPlayerlist = new Player[ThisRoomInfo.MaxPlayers];

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].ActorNumber != -1) ThisRoomPlayerlist.Add(PhotonNetwork.PlayerList[i]);
        }

        //ThisRoomPlayerlist = PhotonNetwork.CurrentRoom.Players;
       // Debug.Log(PhotonNetwork.CurrentRoom.Players.Values);
        // Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(0,true).NickName);
        // Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(1,true).NickName);
        for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            GameObject Temp = Instantiate(PlayerPrefeb,PlayerParent.transform);
            //Player TempPlayer = PhotonNetwork.CurrentRoom.GetPlayer();
            Temp.transform.Find("PlayerName").gameObject.GetComponent<Text>().text = ThisRoomPlayerlist[i].NickName;
            //Temp.transform.Find("RoomMax").gameObject.GetComponent<Text>().text = $"{Mylist[GetRoomNum(RoomName)].PlayerCount}/4";
        }

        
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

    public override void OnJoinedRoom()
    {
        Debug.Log($"방 입장 완료");
        ShowMePlayerList();
        //Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(0,true));
        
    }
    
}
