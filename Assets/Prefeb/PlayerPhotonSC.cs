using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerPhotonSC : MonoBehaviour, IPunObservable
{
    //string tlqkf = "sd";
    List<GameObject> PlayerList = new List<GameObject>();
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //stream.SendNext();
        }
        else 
        { 
           // Debug.Log((string)stream.ReceiveNext());
            // SetReady((string)stream.ReceiveNext()).GetComponent<Text>().text = "dd";
        }
    }

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetReady(string PlayerName,bool Ready)
    {
        bool allReady = true;

        foreach (Transform child in transform)
        {
            PlayerList.Add(child.gameObject);
        }

        foreach (GameObject player in PlayerList)
        {
            if (player.transform.GetChild(1).gameObject.GetComponent<Text>().text == PlayerName)
            {
                string Temp = Ready ? "준비완료" : "";
                player.transform.GetChild(0).gameObject.GetComponent<Text>().text = Temp;
                
            }

            if (player.transform.GetChild(0).gameObject.GetComponent<Text>().text != "준비완료")
            {
            allReady = false;
            }
        }

        if (allReady)
        {
        // 모든 플레이어가 준비 완료했을 때 할 작업
            Invoke(nameof(LetsGoInGame),5f);
            NetworkManager.networkManager.StartLoadingPanel.SetActive(true);
            //NetworkManager.networkManager.SetInvisibleRoom();
        }
    }

    void LetsGoInGame()
    {
        SceneManager.LoadScene("MultiScene");
    }
}
