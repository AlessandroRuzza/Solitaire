using System;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public Action cardPlacedOnPile = null;
    public Source source;
    Vector3 offset = Vector3.zero;
    Vector3 spawnPos;
    public bool isOnCorrectPile = false;
    public Vector3 pilePos;
    public bool blockMovement = false;

    void OnMouseDown()
    {
        if (blockMovement) return;
        offset = transform.position - getMousePos();
        spawnPos = transform.position;

        transform.position += Vector3.back * 100;
    }

    void OnMouseDrag()
    {
        if (blockMovement) return;
        transform.position = getMousePos() + offset;
    }

    void OnMouseUp()
    {
        if (blockMovement) return;
        if (!isOnCorrectPile)
            transform.position = spawnPos;
        else
        {
            transform.position = pilePos;
            blockMovement = true;
            if (cardPlacedOnPile != null) cardPlacedOnPile();
        }
    }

    Vector3 getMousePos()   // get mouse position in world coords, ignoring z
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        return mousePos;
    }
}
