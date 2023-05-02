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
    GameObject Lobby;

    [SerializeField]
    GameObject LobbyRoomCount, NoRoomIndicator, CreateRoom, RoomPrefeb;

    [SerializeField]
    GameObject PlayerParent, PlayerPrefeb, RoomPanel;
    
    string  NickName, RoomName;
    


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
       
        
    }

    public void LoginInfoCallBack()
    {
        NickName = BringNickname.GetComponent<Text>().text;
    }

    public void PopupLobby()
    {
        NetworkManager.PlayerNAME = PopUp.transform.Find("NameField").Find("NameText").gameObject.GetComponent<Text>().text;
        NetworkManager.JoinLobby();
        PopUp.SetActive(false);
        Lobby.SetActive(true);

        NetworkManager.RoomPrefeb = this.RoomPrefeb;
        NetworkManager.RoomParent = this.LobbyRoomCount;

        NetworkManager.PlayerPrefeb = this.PlayerPrefeb;
        NetworkManager.PlayerParent = this.PlayerParent;
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
}
