using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformPitch : MonoBehaviour
{
    //public Transform AudioAnalyser;
    public Vector2 ppos;
    public PitchAnalyzer analyzer;
    public float divisor;
    public float midLevel;

    void Start()
    {
    }

    void Update()
    {
        GameObject theCamera = GameObject.Find("Pitch Input");
        PitchAnalyzer audioAnalyse = theCamera.GetComponent<PitchAnalyzer>();
        if(audioAnalyse.smoothedPitch<midLevel && audioAnalyse.smoothedPitch>0.1)
        {
            ppos = new Vector2(transform.position.x, -audioAnalyse.smoothedPitch / divisor);
        }
        else
        {
            ppos = new Vector2(transform.position.x, audioAnalyse.smoothedPitch / divisor);
        }
        
        transform.position = ppos;
    }
}
