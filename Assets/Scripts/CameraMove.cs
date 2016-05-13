using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{

    private Transform player;
    private Transform myTrans;
    private Vector3 currentVelocity = Vector3.zero;

    public float camaraTurn = 2f;
    //몇초에 도달 할지.
    public float smoothTime = 1f;

    // Use this for initialization
    void Awake()
    {
        myTrans = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    /*/void LateUpdate () {
        myTrans.position = player.position;
	}/*/

    void LateUpdate()
    {
        myTrans.position = Vector3.SmoothDamp(myTrans.position, player.position, ref currentVelocity, smoothTime);

        //myTrans.rotation = player.rotation;
        //myTrans.rotation = Quaternion.Slerp(myTrans.rotation, player.rotation, camaraTurn * Time.deltaTime);
    }
}
