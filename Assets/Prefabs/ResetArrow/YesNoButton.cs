using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YesNoButton : MonoBehaviour
{
    [SerializeField] bool isYesButton;
    [SerializeField] GameObject window;

    private void OnMouseDown()
    {
        if (isYesButton)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            window.SetActive(false);
        }
    }
}
