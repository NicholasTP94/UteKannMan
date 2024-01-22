using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 1f;
    [SerializeField] float leadingDist = 0f;

    [SerializeField] Transform player;
    Transform thisCameraTransform;

    Vector3 startPos;
    Vector3 tempPos;

    float inpHor;
    float inpVer;
    float tempHor = 1f;
    float tempVer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Find object by component
        player = FindObjectOfType<PlayerController2D>().transform;

        thisCameraTransform = Camera.main.transform;

        startPos = thisCameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        inpHor = Input.GetAxis("Horizontal");
        inpVer = Input.GetAxis("Vertical");
        if (inpHor < 0) tempHor = -1f;
        else if (inpHor > 0) tempHor = 1f;

        if (inpVer > 0) tempHor = 1f;
        else if (inpVer > 0) tempVer = -1f;


        tempPos = player.position;
        tempPos.z = startPos.z; // How far the camera is
        tempPos.y += 3 + tempVer * leadingDist;
        tempPos.x += tempHor * leadingDist;

        //thisCameraTransform.position = Vector3.Slerp(thisCameraTransform.position, tempPos, Time.deltaTime);
        thisCameraTransform.position = Vector3.MoveTowards(thisCameraTransform.position, tempPos, cameraSpeed);

        // Changing a single value (x,y,z)
        //thisCameraTransform.position.z = 10f;   // Error. You cannot change a single value (x,y,z)
        //thisCameraTransform.position = new Vector3(thisCameraTransform.position.x, thisCameraTransform.position.x, -10f);
    }
}