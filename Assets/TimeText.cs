using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    [SerializeField] float TextTime;
    [SerializeField] Text ShowTime; 

    // Start is called before the first frame update
    void OnEnable()
    {
        TextTime = 6f;
        ShowTime = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        TextTime -= Time.deltaTime;
        ShowTime.text = $"{(int)TextTime}초 뒤에 게임을 시작합니다."; 
    }
}
