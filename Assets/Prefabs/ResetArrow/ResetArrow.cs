using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetArrow : MonoBehaviour
{
    [SerializeField] GameObject confirmWindow;

    private void Awake()
    {
        confirmWindow.SetActive(false);
    }

    private void OnMouseDown()
    {
        confirmWindow.SetActive(true);
    }

}
