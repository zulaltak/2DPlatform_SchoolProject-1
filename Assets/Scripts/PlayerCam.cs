using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform target;
    public float targetAhead;
    [Range(1,10)]
    public float camSpeed;
    Vector3 startPos;
    float startTargetPosY;

    private void Start()
    {
        startPos = transform.position;
        startTargetPosY = target.position.y;
    }

    void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = new Vector3(target.position.x + (targetAhead * target.transform.localScale.x), startPos.y, transform.position.z);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, camSpeed * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
