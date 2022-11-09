using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTarget : MonoBehaviour
{

    public Transform roomCentre;
    public Transform targetObject;

    [Range(0, 1)]
    public float lerpPercent;

    Vector3 cameraTargetPosition;
    public float cameraRotationSpeed;

    void Update()
    {
        cameraTargetPosition = roomCentre.position + lerpPercent * (targetObject.position - roomCentre.position);
        cameraTargetPosition.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(cameraTargetPosition - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotationSpeed * Time.deltaTime);
    }
}