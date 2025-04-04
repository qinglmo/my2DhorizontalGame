using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform mTarget;
    private void Start()
    {
        mTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        transform.localPosition = new Vector3(mTarget.position.x, mTarget.position.y,-10);

    }
}
