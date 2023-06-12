using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource ThisEar;
    public AudioClip BGM;
    // Start is called before the first frame update
    void Start()
    {
        ThisEar =  GetComponent<AudioSource>();
        
       // BGM = GetComponent<AudioClip>();
        ThisEar.PlayOneShot(BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
