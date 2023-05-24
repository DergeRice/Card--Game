using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject OrginCanvas,MovingCanvas,TableCanvas;

    
    // Start is called before the first frame update

    public static DragAndDrop dragAndDrop;
    void Start()
    {
        dragAndDrop = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMCToOC(GameObject Parent)
    {
        MovingCanvas = Instantiate(OrginCanvas,Parent.transform);
        
        for (int i = 0; i < MovingCanvas.transform.childCount; i++)
        {
            Destroy(MovingCanvas.transform.GetChild(i).gameObject);
        }
        //MovingCanvas.GetComponent<Canvas>().sortingOrder = 20;
        MovingCanvas.transform.position = OrginCanvas.transform.position;
        MovingCanvas.transform.rotation = OrginCanvas.transform.rotation;
        MovingCanvas.name = "MovingCanvas";
       // .transform.position = OrginCanvas.transform.position;
    }
}
