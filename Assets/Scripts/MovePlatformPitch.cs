using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformPitch : MonoBehaviour
{
    //public Transform AudioAnalyser;
    public Vector2 ppos;
    public PitchAnalyzer audioAnalyse;
    public float divisor;
    public float midLevel;
    public bool isHorizontal;

    [Header("Rigidbody controls")]
    public float force;
    public float maxTravelDistance;
    public float mass;


    private Rigidbody2D rb2D;
    private Vector3 initialPos;


    void Start()
    {
        midLevel = audioAnalyse.midLevel;
        /*rb2D = gameObject.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
        rb2D.mass = mass;
        rb2D.drag = 1;
        rb2D.angularDrag = 1;
        rb2D.freezeRotation = true;*/
        initialPos = transform.position;
    }

    void FixedUpdate()
    {
       // GameObject theCamera = GameObject.Find("Main Camera");
       // PitchAnalyzer audioAnalyse = theCamera.GetComponent<PitchAnalyzer>();
        Vector3 distanceTravelled = transform.position - initialPos;
        //Debug.Log(distanceTravelled);

        if (!isHorizontal)
        {

            if (audioAnalyse.smoothedPitch < midLevel && audioAnalyse.smoothedPitch > 10 && distanceTravelled.y > (-maxTravelDistance))
            {
                //rb2D.AddForce(transform.up * -force);
                transform.Translate(Vector3.up * -force * Time.deltaTime, Space.World);
                //ppos = new Vector2(transform.position.x, -audioAnalyse.smoothedPitch / divisor);
            }
            else if (audioAnalyse.smoothedPitch > midLevel && distanceTravelled.y < (maxTravelDistance))
            {
                //rb2D.AddForce(transform.up * force);
                transform.Translate(Vector3.up * force * Time.deltaTime, Space.World);
                //ppos = new Vector2(transform.position.x, audioAnalyse.smoothedPitch / divisor);
            }


        }
        else
        {
            if (audioAnalyse.smoothedPitch < midLevel && audioAnalyse.smoothedPitch > 10 && distanceTravelled.x > (-maxTravelDistance))
            {
                //rb2D.AddForce(transform.right * -force);
                transform.Translate(Vector3.right * -force * Time.deltaTime, Space.World);
                //ppos = new Vector2(transform.position.x, -audioAnalyse.smoothedPitch / divisor);
            }
            else if (audioAnalyse.smoothedPitch > midLevel && distanceTravelled.x < (maxTravelDistance))
            {
                //rb2D.AddForce(transform.right * force);
                transform.Translate(Vector3.right * force * Time.deltaTime, Space.World);
                //ppos = new Vector2(transform.position.x, audioAnalyse.smoothedPitch / divisor);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }



    //transform.position = ppos;
}
