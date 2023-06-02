using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPScript : MonoBehaviour
{
   public void SuccessPurchase()
   {
        Debug.Log("Success");
   }
    public void FailPurchase()
   {
        Debug.Log("Fail");
   }
}
