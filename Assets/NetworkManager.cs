using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks , IPunObservable
{
    public PhotonView PV;
    public static string PlayerNAME;
    public static string RoomNAME;
    public static List<RoomInfo> MyRoomList = new List<RoomInfo>();
    //public static List<RoomInfo> MyPlayerlist = new List<RoomInfo>();

    public static GameObject RoomParent { get; set; }
    public static GameObject RoomPrefeb { get; set; }

    public static GameObject PlayerParent { get; set; }
    public static GameObject PlayerPrefeb { get; set; }
    public static GameObject LoadingPannel { get; set; }

    public static List<Player> CurRoomPlayer { get; set; }

    public static GameObject cardDeck;

    public static bool ReadyBtn;
    List<Player> ThisRoomPlayerlist = new List<Player>();


    public GameObject CardDeck {get; set;}
    public static NetworkManager networkManager;

    public GameObject MyHand;
    public List<GameObject> PlayersHand = new List<GameObject>();
    

    private void Start() 
    {
        Screen.SetResolution(1920,1080,false);
    }
    public static void LogInGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Login Success");
        LoadingPannel.SetActive(false);
        if(PlayerNAME != null) NetworkManager.JoinLobby();
    }
    void Awake() 
    {
        networkManager = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LocalPlayer.NickName = PlayerNAME;
        
    }
    

    public override void OnJoinedLobby()
    {
       // Debug.Log("나 로비야");
        ShowMeRoomList(); 

    }

    public static void CreateRoomList(string RoomName)
    {
        PhotonNetwork.CreateRoom(RoomName,new RoomOptions { MaxPlayers = 4 });
        //Debug.Log(RoomName);
        
    }

    public static void JoinRoomList(string RoomName)
    {
       // Debug.Log($"JoinedRoom: {RoomName}");
        PhotonNetwork.JoinRoom(RoomName);
        RoomNAME = RoomName;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       ShowMePlayerList();
    }

    
    public static void RefreshRoom()
    {
        //Debug.Log(PhotonNetwork.PlayerList);
    }


    public override void OnCreatedRoom()
    {
       // Debug.Log($"CreatedRoom: {RoomNAME}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        MyRoomList = roomList;
        for (int i = 0; i < roomCount; i++)
        {
            if(!roomList[i].RemovedFromList)
            {
                if(!MyRoomList.Contains(roomList[i])) MyRoomList.Add(roomList[i]);
                else MyRoomList[MyRoomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if(MyRoomList.IndexOf(roomList[i]) != -1) MyRoomList.RemoveAt(MyRoomList.IndexOf(roomList[i]));
        }

        ShowMeRoomList();
       // Debug.Log(roomList.Count+"update");
    }

    public static int GetRoomNum(string RoomName)
    {
        for (int i = 0; i < MyRoomList.Count; i++)
        {
            if(MyRoomList[i].Name == RoomName) return i;
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
        for (int i = 0; i < MyRoomList.Count; i++)
        {
            GameObject Temp = Instantiate(RoomPrefeb,RoomParent.transform);
            Temp.transform.Find("RoomName").gameObject.GetComponent<Text>().text = MyRoomList[i].Name;
            //Debug.Log(MyRoomList[GetRoomNum(RoomName)]);
            Temp.transform.Find("RoomMax").gameObject.GetComponent<Text>().text = $"{MyRoomList[i].PlayerCount}/4";
        }
        Debug.Log("RoomChanged"+$"CurrentRoomCount:{MyRoomList.Count}"); 
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ShowMePlayerList();
    }
    
    public static void ShowMePlayerList()
    {
        Dictionary<int,Player> ThisRoomPlayerlist = new Dictionary<int, Player>();

        //RoomInfo ThisRoomInfo = MyRoomList[GetRoomNum(RoomNAME)];

        if(PlayerParent.transform.childCount != 0)
        {
            for (int i = 0; i < PlayerParent.transform.childCount; i++)
            {
                Destroy(PlayerParent.transform.GetChild(i).gameObject);
            }
        }
        

        // for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        // {
        //     if(PhotonNetwork.PlayerList[i].ActorNumber != -1) ThisRoomPlayerlist.Add(PhotonNetwork.PlayerList[i]);
        // }
        ThisRoomPlayerlist = PhotonNetwork.CurrentRoom.Players;

        foreach (var player in ThisRoomPlayerlist.Values)
        {
            GameObject temp = Instantiate(PlayerPrefeb, PlayerParent.transform);
            temp.transform.Find("PlayerName").gameObject.GetComponent<Text>().text = player.NickName;
        }
       // Debug.Log($"Last Player Count: {ThisRoomPlayerlist.Count}");
        
    }

    public static string SetMyMaxPlayer(string RoomName)
    {
        for (int i = 0; i < MyRoomList.Count; i++)
        {
            if(MyRoomList[i].Name == RoomName){
                return MyRoomList[i].PlayerCount + "/4";
            }
        }
         return "";
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log($"방 입장 완료");
        ShowMePlayerList();
        //Debug.Log(PhotonNetwork.CurrentRoom.GetPlayer(0,true));
        
    }
    public static void LeaveRoomList()
    {
        // if(PhotonNetwork.IsMasterClient)
        // {
        //     PhotonNetwork.SetMasterClient(PhotonNetwork.CurrentRoom.);
        // }

        //if(PhotonNetwork.InRoom) NetworkManager.JoinLobby();
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // PhotonNetwork.CurrentRoom.IsOpen = false;
            // PhotonNetwork.CurrentRoom.IsVisible = false;
            // PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            //Debug.Log("문 닫고 나간다");
            MyRoomList.Remove(PhotonNetwork.CurrentRoom);
            //PhotonNetwork.CurrentRoom.EmptyRoomTtl = 1;
            //MyRoomList.Remove(PhotonNetwork.CurrentRoom);
        }
        
        PhotonNetwork.LeaveRoom(true);
        // NetworkManager.JoinLobby();
        
        //Debug.Log("나 나간다");
    }

    public override void OnLeftRoom()
    {
       // NetworkManager.JoinLobby();
        //OnRoomListUpdate(MyRoomList);
    }

    [PunRPC]
     public void SetReady(string PlayerName, bool Ready)
     {
        PlayerParent.GetComponent<PlayerPhotonSC>().SetReady(PlayerName,Ready);       
     }

    

    public void ReadBtnEntered()
    {
        ReadyBtn =! ReadyBtn;
        PV.RPC("SetReady",RpcTarget.All,PlayerNAME,ReadyBtn);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {   
        if(stream.IsWriting)
        {
            //stream.SendNext(PlayerNAME);
        }
        else 
        { 
           // Debug.Log((string)stream.ReceiveNext());
            // SetReady((string)stream.ReceiveNext()).GetComponent<Text>().text = "dd";
        }
    }

//     public class PlayerStat
//     {
//        public string PlayerName;
//        public bool Ready;
//     }
    public static void StartGame()
    {
        // if(PhotonNetwork.roo)
        // {
        //     GameManager.StartGame();
        // }
        //GameManager.StartGame();
    }

    #region InGame

    public void GiveCard(int CardNum,int PlayerNum)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PV.RPC("SpreadCardInfo", RpcTarget.AllBuffered, CardNum, PlayerNum);
        }
        
    }

    [PunRPC]
    public void SpawnCardDeck()
    {
        //cardDeck = Instantiate(CardDeck);
        //cardDeck.name = "ThisGame";
        //Debug.Log("Spawned");
    }

    [PunRPC]
    public void SpreadCardInfo(int CardNum,int PlayerNum)
    {
        GameObject CardDeck = GameObject.Find("CardDeck");
        Debug.Log(CardNum+"/"+CardDeck.transform.childCount);
        GameObject Card = CardDeck.transform.GetChild(CardNum).gameObject;
        
        Card.transform.parent = PlayersHand[PlayerNum].GetComponent<PlayerScript>().HandCanvas.transform;
        Card.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Card.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,0);
        //Card.transform.rotation = Quaternion.Euler(0,0,0);
        Card.transform.localScale = new Vector3(2,2,2);
    }


    
    #endregion 
}
