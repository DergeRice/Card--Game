using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCreator : MonoBehaviour
{
    [SerializeField]
    Sprite[] CardImg = new Sprite[12*5];

    [SerializeField]
    GameObject CardPrefeb,CardParent;
    [ContextMenu("dd")]
    public GameObject MakeCard()
    {
        GameObject MadeCard = Instantiate(CardPrefeb,CardParent.transform);
        MadeCard.transform.GetChild(0).GetComponent<Image>().sprite = CardImg[Random.Range(0,12*5)];

        return MadeCard;
    }
}
