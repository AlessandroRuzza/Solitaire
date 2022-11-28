using System;
using UnityEngine;

public class DragController : MonoBehaviour
{
    static Action setSpawnPos = null;
    public Action cardPlacedOnPile = null;
    public Action cardPlacedOnCard = null;
    public Action cardPlacedOnCol = null;
    public Source source;
    Vector3 offset = Vector3.zero;
    [SerializeField] Vector3 spawnPos;
    public bool isOnCorrectPile = false;
    public bool isOnCorrectCard = false;
    public bool isOnCorrectCol = false;
    public Vector3 pilePos;
    public Vector3 cardPos;
    public Vector3 colPos;
    public bool blockMovement = false;

    private void Awake()
    {
        setSpawnPos += SetSpawnPos;
    }
    private void OnDestroy()
    {
        setSpawnPos -= SetSpawnPos;
        cardPlacedOnPile = null;
        cardPlacedOnCard = null;
        cardPlacedOnCol = null;
    }
    void SetSpawnPos()
    {
        spawnPos = transform.position;
    }

    void OnMouseDown()
    {
        if (blockMovement) return;
        offset = transform.position - getMousePos();
        SetSpawnPos();

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
        if (isOnCorrectPile)                    // placement priority:  Pile >> Card >> Col
        {
            transform.position = pilePos;
            blockMovement = true;
            transform.SetParent(null);
            if (cardPlacedOnPile != null) cardPlacedOnPile();
            isOnCorrectPile = false;
        }
        else if (isOnCorrectCard)
        {
            transform.position = cardPos;
            if (cardPlacedOnCard != null) cardPlacedOnCard();
            SetAllSpawnPos(name);
            isOnCorrectCard = false;
        }
        else if (isOnCorrectCol)
        {
            transform.position = colPos;
            if (cardPlacedOnCol != null) cardPlacedOnCol();
            SetAllSpawnPos(name);
            isOnCorrectCol = false;
        }
        else
        {
            transform.position = spawnPos;
        }
        SetSpawnPos();  // in any case, update spawnPos
    }

    Vector3 getMousePos()   // get mouse position in world coords, ignoring z
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        return mousePos;
    }
    static void SetAllSpawnPos(string name)
    {
        if (setSpawnPos != null) setSpawnPos();  // calls Action: sets spawnPos of every Card on the field to its transform.position
        else Debug.LogError("setSpawnPos was null! called by " + name);
        // setSpawnPos should never be empty: it invokes at least this instance's function

    }
}
