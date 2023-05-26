using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
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
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            MyRoomList.Remove(PhotonNetwork.CurrentRoom);
        }
        PhotonNetwork.LeaveRoom(true);
    }

    public override void OnLeftRoom()
    {

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
        }
        else 
        { 
        }
    }

    #region InGame

    public void GiveCard(int CardNum,int PlayerNum, int CardAmount)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PV.RPC("SpreadCardInfo", RpcTarget.AllBuffered, CardNum, PlayerNum,CardAmount);
        }
    }
    public void GiveCardClient(int CardNum,int PlayerNum, int CardAmount)
    {
        PV.RPC("ClientGetCard", RpcTarget.AllBuffered, CardNum, PlayerNum,CardAmount);
    }
    
    [PunRPC]
    public void ClientGetCard(int CardNum,int PlayerNum, int CardAmount)
    {
        GiveCard(CardNum,PlayerNum,CardAmount);
    }

    public int GetMyPlayerNum()
    {

        for (int i = 0; i < PlayersHand.Count; i++)
        {
            if(PlayersHand[i].GetComponent<PlayerScript>().IsMine) return i;
        }
         
        return -1;
    
        
    }

    public void SetInitialFun(int PlayerNum)
    {
        PV.RPC("SetInitial", RpcTarget.AllBuffered,PlayerNum);
    }

    [PunRPC]
    public void SetInitial(int PlayerNum)
    {
        //if(PlayerNum == -1) return;
        PlayersHand[PlayerNum].GetComponent<PlayerScript>().SetMyProfile(Random.Range(0,30)); //profile 설정
    }
    
    [PunRPC]
    public void SpreadCardInfo(int CardNum,int PlayerNum,int CardAmount)
    {
        if(PlayerNum == -1) return;
        StartCoroutine(GiveCardCo(CardNum,PlayerNum,CardAmount));
    }

    IEnumerator GiveCardCo(int CardNum,int PlayerNum,int CardAmount)
    {
        GameObject CardDeck = GameObject.Find("CardDeck");
        GameObject playerHand = PlayersHand[PlayerNum].GetComponent<PlayerScript>().HandCanvas;
        //GameObject Card = CardDeck.transform.GetChild(CardNum).gameObject;
        for (int i = 0; i < CardAmount; i++)
        {
            CardNum %= CardDeck.transform.childCount;
            GameObject Card = CardDeck.transform.GetChild(CardNum).gameObject;

            Card.GetComponent<CardScript>().PlayDrawAnimation();
            yield return new WaitForSeconds(0.5f); // 1초 대기

            
            Card.transform.parent = playerHand.transform;
            Card.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Card.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            Card.transform.localScale = new Vector3(2, 2, 2);

            CardNum++; // 다음 카드 번호로 이동
        }
    }


    
    #endregion 
}

[InitializeOnLoad]
public class EditorStartInit
{
    static EditorStartInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path; // 씬 번호를 넣어주자.
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
        Debug.Log(pathOfFirstScene + " 씬이 에디터 플레이 모드 시작 씬으로 지정됨");
    }
}
