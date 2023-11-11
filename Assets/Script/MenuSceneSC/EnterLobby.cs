using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterLobby : MonoBehaviour
{

    [SerializeField]
    GameObject PopUp;
    GameObject BringNickname;

    [SerializeField]
    GameObject Lobby,NickNameError;

    [SerializeField]
    GameObject LobbyRoomCount, NoRoomIndicator, CreateRoom, RoomPrefeb;

    [SerializeField]
    GameObject PlayerParent, PlayerPrefeb, RoomPanel, LoadingPannel;
    
    string  NickName, RoomName;


    private void Start() {
        NetworkManager.LoadingPannel = this.LoadingPannel;
        NetworkManager.RoomPrefeb = this.RoomPrefeb;
        NetworkManager.RoomParent = this.LobbyRoomCount;

        NetworkManager.PlayerPrefeb = this.PlayerPrefeb;
        NetworkManager.PlayerParent = this.PlayerParent;
    }
    private void Update() 
    {
        NoRoomCheck();
    }

    void NoRoomCheck()
    {
        if(LobbyRoomCount.transform.childCount == 0)
        {
            NoRoomIndicator.SetActive(true);
        }
        else{ NoRoomIndicator.SetActive(false); }
    }
    public void MultiPopup()
    {
        PopUp.SetActive(true);
        NetworkManager.LogInGame();
       LoadingPannel.SetActive(true);
        
    }

    public void LoginInfoCallBack()
    {
        NickName = BringNickname.GetComponent<Text>().text;
    }

    public void PopupLobby()
    {
        NetworkManager.PlayerNAME = PopUp.transform.Find("NameField").Find("NameText").gameObject.GetComponent<Text>().text;


        string temp = NetworkManager.networkManager.CanIGoIn();
        if (temp != "") 
        {
            //NickNameError.SetActive(true);
            //NickNameError.GetComponent<Text>().text = temp;
            return;
        }
        
        NetworkManager.JoinLobby();
        PopUp.SetActive(false);
        Lobby.SetActive(true);
        RoomPanel.SetActive(false);

    }

    public void PopupCreateRoom()
    {
        CreateRoom.SetActive(true);
    }
    public void CancelCreateRoom()
    {
        CreateRoom.SetActive(false);
    }
    public void ConfirmCreateRoom()
    {
        RoomName = CreateRoom.transform.Find("RoomNameInput").Find("RoomNameText").GetComponent<Text>().text;
        if (RoomName == "") return;
        CreateRoom.SetActive(false);
        Lobby.SetActive(false);
        RoomPanel.SetActive(true);
        NetworkManager.CreateRoomList(RoomName);
    }

    public void EnterRoom(string RoomName)
    {
        Lobby.SetActive(false);
        RoomPanel.SetActive(true);
        NetworkManager.JoinRoomList(RoomName);
    }


    public void ExitFromRoom()
    {
        PopupLobby();
        NetworkManager.LeaveRoomList();
        
        //NetworkManager.room
    }

}
