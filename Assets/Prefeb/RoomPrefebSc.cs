using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPrefebSc : MonoBehaviour
{
    [SerializeField]
    GameObject TextOBJ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThisRoomEnter()
    {
        GameObject.Find("SystemController").GetComponent<EnterLobby>().EnterRoom(TextOBJ.GetComponent<Text>().text);
    }
}
