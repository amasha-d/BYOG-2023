using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target1;
    //public GameObject target2;
    public Vector3 target;
    public Vector3 offset;
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;


    void FixedUpdate()
    {
       /* if (target1.activeSelf == false)
        {
            target1 = target2;
        }
        else if (target2.activeSelf == false)
        {
            target2 = target1;
        }*/
        target = (target1.transform.position) ;
        Vector3 targetPosition = new Vector3(target.x + offset.x, target.y + offset.y, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
