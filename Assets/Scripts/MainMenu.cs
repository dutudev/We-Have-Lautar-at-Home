using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlsTab, mainMenu;

    [SerializeField] private CanvasGroup selectMenu;
    // Start is called before the first frame update
    void Start()
    {
        //mainMenu.transform.localPosition = new Vector3(-1940, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controlsTab.SetActive(false);
        }
    }
    
    public void OpenControls()
    {
        controlsTab.SetActive(true);
    }

    public void OpenSelect()
    {
        selectMenu.alpha = 0;
        selectMenu.gameObject.SetActive(true);
        if (!mainMenu.LeanIsTweening())
        {
            LeanTween.moveLocalX(mainMenu, -1940, 1f).setEaseOutExpo();
            LeanTween.alphaCanvas(selectMenu, 1, 1f).setEaseOutExpo().setDelay(0.4f);
        }
    }
}
