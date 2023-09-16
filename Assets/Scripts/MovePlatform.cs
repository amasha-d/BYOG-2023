using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    //public Transform AudioAnalyser;
    public Vector2 ppos;
    public AudioAnalyzer analyzer;
    public float divisor;

    void Start()
    {
    }

    void Update()
    {
        GameObject theCamera = GameObject.Find("Audio Input");
        AudioAnalyzer audioAnalyse = theCamera.GetComponent<AudioAnalyzer>();

        ppos = new Vector2(transform.position.x, audioAnalyse.loudness/divisor);
        transform.position = ppos;
    }
}
