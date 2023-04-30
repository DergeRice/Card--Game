using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCurrentIndicator : MonoBehaviour
{
    [SerializeField]
    GameObject ThisRoomNameObject;

    string ThisRoomNameString;

    
    private void OnEnable() 
    {
        ThisRoomNameString = ThisRoomNameObject.GetComponent<Text>().text;
    }
    void Update()
    {
        
    }

    void SetMyMaxPlayerInd()
    {
        GetComponent<Text>().text = NetworkManager.SetMyMaxPlayer(ThisRoomNameString);
    }
}
