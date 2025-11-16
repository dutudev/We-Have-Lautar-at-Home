using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private Sprite[] images;

    [SerializeField] private Image imageUI;
    private CanvasGroup _imageCanvas;
    private int _index;
    // Start is called before the first frame update
    void Start()
    {
        _imageCanvas = imageUI.gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _index++;
            if (_index == images.Length)
            {
                LeanTween.alphaCanvas(_imageCanvas, 0, 1f).setOnComplete(() =>
                {
                    SceneManager.LoadScene("MainMenu");
                });
                return;
            }

            if (_index < images.Length)
            {
                imageUI.sprite = images[_index];
            }
        }
    }
}
