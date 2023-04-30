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
        NetworkManager.RoomNAME = RoomName;
        NetworkManager.CreateRoomList();
        RoomName = CreateRoom.transform.Find("RoomNameInput").Find("RoomNameText").GetComponent<Text>().text;
        GameObject Temp = Instantiate(RoomPrefeb,LobbyRoomCount.transform);
        Temp.transform.Find("RoomName").gameObject.GetComponent<Text>().text = RoomName;
        
        
        //NetworkManager.JoinRoomList(RoomName);
        CreateRoom.SetActive(false);
    }

    public void EnterRoom()
    {
        
    }
}
