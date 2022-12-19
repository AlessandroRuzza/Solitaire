using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ScreenBoundsPlacer : MonoBehaviour
{
    [SerializeField] bool placeFromTop;
    [SerializeField] bool placeFromLeft;
    [SerializeField] float verticalOffset;
    [SerializeField] float lateralOffset;

    void Awake()
    {
        // use half size so the screen coords are (-size -> +size)
        float vertCameraSize = Camera.main.orthographicSize;       
        float horizCameraSize = vertCameraSize * Camera.main.aspect;

        Vector3 finalPosition = Vector3.zero;

        float xCoord = horizCameraSize - lateralOffset;
        float yCoord = vertCameraSize - verticalOffset;

        if (placeFromLeft) finalPosition.x = -xCoord;   // left side has x < 0
        else finalPosition.x = xCoord;

        if (placeFromTop) finalPosition.y = yCoord;     // top side has y > 0
        else finalPosition.y = -yCoord;

        finalPosition.z = transform.position.z;

        transform.position = finalPosition;
    }
}
