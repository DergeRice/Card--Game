
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject HandCanvas;
    public GameObject ParentObj;
    public bool IsMine = false;

    // Start is called before the first frame update
    void Start()
    {
        if(IsMine)
        {
            DragAndDrop.dragAndDrop.OrginCanvas = HandCanvas;
            DragAndDrop.dragAndDrop.SetMCToOC(ParentObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
