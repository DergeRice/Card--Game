using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using UnityEditor.SceneManagement;
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
    public GameObject StartLoadingPanel;

    public static List<Player> CurRoomPlayer { get; set; }

    public static GameObject cardDeck;

    public static bool ReadyBtn;
    List<Player> ThisRoomPlayerlist = new List<Player>();


    public GameObject CardDeck {get; set;}
    public static NetworkManager networkManager;

    public GameObject MyHand;
    public int MyPlayerNum;
    public List<GameObject> PlayerCanvas = new List<GameObject>();
    public List<GameObject> PlayersIndicator = new List<GameObject>();

    public Vector3 MyHandPos;

    public bool IsGameStart = false;

    public int EventTargetNum = -1;
    public GameObject RobbedCard,ExchangeHostCard,ExchangeSubCard;

    public int ExchangeOwnerNum,ExchangeSubNum;
    

    private void Start() 
    {
        
        Screen.SetResolution(1920,1080,false);
    }
    public void Setting()
    {
        MyPlayerNum = MyHand.GetComponent<MyHandScript>().MyPlayerNum-1;
    }
    public static void LogInGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Login Success");
        LoadingPannel.SetActive(false);
        NetworkManager.JoinLobby();
    }
    void Awake() 
    {
        networkManager = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void JoinLobby()
    {
        LoadingPannel.SetActive(true);
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LocalPlayer.NickName = PlayerNAME;
        
    }

    public string CanIGoIn() 
    {
        Debug.Log(PhotonNetwork.CountOfPlayers);

        if (PhotonNetwork.CountOfPlayers > 1)
        {
            for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
            {
                if (PhotonNetwork.PlayerList[i].NickName == PlayerNAME) return "이미 사용중인 닉네임입니다.";
            }
        }
        if (PlayerNAME == "") return "닉네임을 지어주세요";

        return "";
    }
    

    public override void OnJoinedLobby()
    {
       // Debug.Log("나 로비야");
        ShowMeRoomList();
        LoadingPannel.SetActive(false);
    }

    public static void CreateRoomList(string RoomName)
    {
        PhotonNetwork.CreateRoom(RoomName,new RoomOptions { MaxPlayers = 4 });
        //Debug.Log(RoomName);
        
    }

    public static void JoinRoomList(string RoomName)
    {
        // Debug.Log($"JoinedRoom: {RoomName}");
        LoadingPannel.SetActive(true);
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
        LoadingPannel.SetActive(false);

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

    public void SetInvisibleRoom() 
    {
        PhotonNetwork.CurrentRoom.IsVisible = false;
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
            PV.RPC(nameof(SpreadCardInfo), RpcTarget.AllBuffered, CardNum, PlayerNum,CardAmount);
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
        
        for (int i = 0; i < PlayerCanvas.Count; i++)
        {
            if(PlayerCanvas[i].GetComponent<PlayerScript>().IsMine) return i;
        }
         
        return -1;
    }

    public string GetPlayerName(int num)
    {
        return PhotonNetwork.PlayerList[num].NickName;
    }

    public void SetInitialFun()
    {
        PV.RPC(nameof(SetInitial), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetInitial()
    {
        // for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
        // {
        //    if(PlayerCanvas[i].GetComponent<PlayerScript>().MyNum == i)
        //    {
        //         PlayerCanvas[i].GetComponent<PlayerScript>().IsMine = true;
        //    }
        // }
    }
    
    [PunRPC]
    public void SpreadCardInfo(int CardNum,int PlayerNum,int CardAmount)
    {
        if(PlayerNum == -1) return;
        if(IsGameStart)
        {
            
            PlayerCanvas[PlayerNum].GetComponent<PlayerScript>().SetMyProfile(Random.Range(0,30));
            IsGameStart = true;
        } //profile 설정
        StartCoroutine(GiveCardCo(CardNum,PlayerNum,CardAmount));
    }

    IEnumerator GiveCardCo(int CardNum,int PlayerNum,int CardAmount)
    {
        GameObject CardDeck = GameObject.Find("CardDeck");
        GameObject playerHand = PlayerCanvas[PlayerNum].GetComponent<PlayerScript>().HandCanvas;
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

    public void ShowHandAndOutHand(int Show)
    {
        for (int i = 0; i < PlayerCanvas.Count; i++)
        {
            if(PlayerCanvas[i].transform.position != new Vector3(100,100,100))
            {
                PlayerCanvas[i].transform.position = new Vector3(100,100,100);
            }
        }
        PlayerCanvas[Show].transform.position = Vector3.zero;
    }
    public void ShowHandAndOutHand(GameObject Show)
    {
        for (int i = 0; i < PlayerCanvas.Count; i++)
        {
            if(PlayerCanvas[i].transform.position != new Vector3(100,100,100))
            {
                PlayerCanvas[i].transform.position = new Vector3(100,100,100);
            }
        }
        Show.transform.position = Vector3.zero;
    }
    public int GetThisIndicatorNum(GameObject Indi)
    {
        for (int i = 0; i < PlayerCanvas.Count; i++)
        {
            if(PlayersIndicator[i] == Indi) return i;
        }
        return -1;
    }


    public void RobConfirm()
    {
        for (int i = 0; i < PlayersIndicator.Count; i++)
        {
            PlayersIndicator[i].GetComponent<Button>().enabled = false;
        }
        EventCanvas.eventCanvas.RobOwner.SetActive(false);
        ShowHandAndOutHand(MyHand.GetComponent<MyHandScript>().MyPlayer);

        PV.RPC(nameof(RobEnd),RpcTarget.All,GetMyPlayerNum(),EventTargetNum-1,RobbedCard.name);

        for (int i = 0; i < PlayerCanvas.Count; i++)
        {
            PlayerCanvas[i].GetComponent<PlayerScript>().Panel.SetActive(false);
            PlayersIndicator[i].GetComponent<InGamePlayerUI>().ShieldIndiCator.SetActive(false);
        }
    }

    

    [PunRPC]
    public void RobEnd(int RobHostPlayerNum,int RobSubPlayerNum,string RobedCardName)
    {
        //Debug.Log(RobHostPlayerNum+"/"+RobSubPlayerNum+"/"+RobedCardName);
        RobbedCard = GameObject.Find(RobedCardName);
        MoveCardOwnerTo(RobHostPlayerNum,RobbedCard);
        if(MyHand == PlayerCanvas[RobSubPlayerNum].GetComponent<PlayerScript>().HandCanvas) //내가 피해자면
        {
            EventCanvas.eventCanvas.RobSub.SetActive(true);
            EventCanvas.eventCanvas.RobEventSub(RobedCardName);
        }
        Destroy(GameObject.Find("Card38"));
    }

    public void ExchangeRequest(int OpNum,string ExMyCardName,string ExOpCardName)
    {
        for (int i = 0; i < PlayersIndicator.Count; i++)
        {
            PlayersIndicator[i].GetComponent<Button>().enabled = false;
        }
        EventCanvas.eventCanvas.ExOwner.SetActive(false);
        ShowHandAndOutHand(MyHand.GetComponent<MyHandScript>().MyPlayer);

        PV.RPC(nameof(ExOwnerEnd),RpcTarget.All,GetMyPlayerNum(),OpNum,ExMyCardName,ExOpCardName);

        for (int i = 0; i < PlayerCanvas.Count; i++)
        {
            PlayerCanvas[i].GetComponent<PlayerScript>().Panel.SetActive(false);
            PlayersIndicator[i].GetComponent<InGamePlayerUI>().ShieldIndiCator.SetActive(false);
        }
    }

    [PunRPC]
    public void ExOwnerEnd(int ExHostPlayerNum,int ExSubPlayerNum,string ExMyCardName,string ExOpCardName)
    {
        
        ExchangeHostCard = GameObject.Find(ExMyCardName);
        ExchangeSubCard = GameObject.Find(ExOpCardName);

        
        ExchangeOwnerNum = ExHostPlayerNum;
        ExchangeSubNum = ExSubPlayerNum;
        if(MyHand == PlayerCanvas[ExchangeSubNum].GetComponent<PlayerScript>().HandCanvas) //내가 피해자면
        {
            EventCanvas.eventCanvas.ExSub.SetActive(true);
            EventCanvas.eventCanvas.ExEventSub(ExMyCardName,ExOpCardName);
        }
    }

    public void ExSubEndResult(bool Success,string DenyMsg)
    {
        PV.RPC(nameof(ExSubEnd),RpcTarget.All,Success,DenyMsg);
    }

    [PunRPC]
    public void ExSubEnd(bool Success, string DenyMsg)
    {
        
        if(Success)
        {
            MoveCardOwnerTo(ExchangeOwnerNum,ExchangeSubCard);
            MoveCardOwnerTo(ExchangeSubNum,ExchangeHostCard);
            EventCanvas.eventCanvas.DenyText.SetActive(false);
            EventCanvas.eventCanvas.SuccessText.SetActive(true);
            Destroy(GameObject.Find("Card18"));
        }
        else
        {
            EventCanvas.eventCanvas.ReceivedDenyMsg(DenyMsg);
        }

        if(MyHand == PlayerCanvas[ExchangeOwnerNum].GetComponent<PlayerScript>().HandCanvas) //Owner function
        {
            EventCanvas.eventCanvas.ExchangeReceive.SetActive(true);
            if(Success)
            {
                EventCanvas.eventCanvas.DenyText.SetActive(false);
                EventCanvas.eventCanvas.SuccessText.SetActive(true);
            }
            else
            {
                EventCanvas.eventCanvas.ReceivedDenyMsg(DenyMsg);
            }
        }
    }
    public void MoveCardOwnerTo(int To,GameObject Card)
    {
        Debug.Log(To+"플레이어에게"+Card.name+"을 줍니다.");
        // Image Board;

        // if(Card.transform.gameObject.TryGetComponent<Image>(out Board))
        // {
        //     Board.color = new Color(0,0,0);
        // }

        Card.transform.parent = PlayerCanvas[To].GetComponent<PlayerScript>().HandCanvas.transform;
        
        Card.transform.localScale = Vector3.one *2;
        Card.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Card.transform.rotation = Quaternion.Euler(90,0,0);
    }

    public string GetMaxCurrentRoom()
    {
        return PhotonNetwork.PlayerList.Length.ToString();
    }

    public void RequsetOneCard()
    {
        PV.RPC(nameof(RequestCard),RpcTarget.All);
    }
    [PunRPC]
    public void RequestCard()
    {
        DealerScript.dealerScript.RequestAmount ++;
        if(DealerScript.dealerScript.RequestAmount >= PhotonNetwork.PlayerList.Length)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                NetworkManager.networkManager.GiveCard(Random.Range(0,GameObject.Find("CardDeck").transform.childCount),i,1); 
            }   
            DealerScript.dealerScript.RequestAmount = 0;
            PV.RPC(nameof(EnableBtn),RpcTarget.All);
        }
        
    }
    [PunRPC]
    public void EnableBtn()
    {
        DealerScript.dealerScript.RequestBtn.GetComponent<Button>().enabled = true;
    }

    public void SetMyPlayerPoint(int PlayerNum, int Point)
    {
        PV.RPC(nameof(SetMyPlayerPointRPC),RpcTarget.All,PlayerNum,Point);
    }

    [PunRPC]
    public void SetMyPlayerPointRPC(int PlayerNum,int Point)
    {
        Debug.Log(PlayerNum+"플레이어의 점수:"+Point);
        PlayerCanvas[PlayerNum].GetComponent<PlayerScript>().SetMyPoint(Point);
    }

    public void IGotFirst(int first)
    {
        PV.RPC(nameof(IAmFirst),RpcTarget.All,first);
    }

    [PunRPC]
    public void IAmFirst(int first)
    {
        if(MyHand == PlayerCanvas[first].GetComponent<PlayerScript>().HandCanvas) // 내가 일등
        {
            EventCanvas.eventCanvas.BoostPanel.SetActive(true);
        }   
        else
        {
            EventCanvas.eventCanvas.BoostSubPanel.SetActive(true);
        }
    }



    
    #endregion 
}

// [InitializeOnLoad]
// public class EditorStartInit
// {
//     static EditorStartInit()
//     {
//         var pathOfFirstScene = EditorBuildSettings.scenes[0].path; // 씬 번호를 넣어주자.
//         var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
//         EditorSceneManager.playModeStartScene = sceneAsset;
//         Debug.Log(pathOfFirstScene + " 씬이 에디터 플레이 모드 시작 씬으로 지정됨");
//     }
// }
