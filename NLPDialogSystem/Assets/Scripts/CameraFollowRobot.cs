using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowRobot : MonoBehaviour {

    public Transform target;
    private Vector3 m_LastTargetPosition;
    public float lookAheadMoveThreshold = 0.0f;
    // Use this for initialization
    void Start () {
        m_LastTargetPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        float xMoveDelta = (target.position - m_LastTargetPosition).x;
        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
        if (updateLookAheadTarget)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
            m_LastTargetPosition = transform.position;
        }
        
    }
}
