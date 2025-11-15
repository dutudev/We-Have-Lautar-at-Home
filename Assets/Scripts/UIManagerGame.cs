using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Random = UnityEngine.Random;

public class UIManagerGame : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText, rateText;
    public static UIManagerGame instance;

    private CanvasGroup _rateTextAlpha;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        _rateTextAlpha = rateText.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreText(int value)
    {
        scoreText.text = value.ToString();
        LeanTween.cancel(scoreText.gameObject); 
        float randomValue = Random.Range(-1f, 1f);
        scoreText.gameObject.transform.rotation = Quaternion.Euler(0, 0, randomValue * 18);
        scoreText.gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        LeanTween.value(scoreText.gameObject, 0, 1, 0.5f).setEaseOutExpo().setOnUpdate((value) =>
        {
            float rotationValue = Mathf.Lerp(randomValue * 18, 0, value);
            float scaleValue = Mathf.Lerp(1.25f, 1f, value);
            scoreText.gameObject.transform.rotation = Quaternion.Euler(0, 0, rotationValue);
            scoreText.gameObject.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }).setOnComplete(() =>
        {
            scoreText.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            scoreText.gameObject.transform.localScale = Vector3.one;
        });
        //UpdateRateText(value);
    }

    public void UpdateRateText(int value)
    {
        print(value);
        Color textCol;
        string text = String.Empty;
        if (value >= 45)
        {
            textCol = new Color(0, 255, 113);
            text = "Perfect";
        }else if (value >= 35)
        {
            textCol = new Color(0, 255, 10);
            text = "Good";
        }else if (value >= 10)
        {
            textCol = new Color(166, 255, 0);
            text = "Ok";
        }
        else
        {
            textCol = new Color(255, 0, 0);
            text = "Miss";
        }

        _rateTextAlpha.alpha = 1;
        rateText.color = textCol;
        rateText.text = text;
        LeanTween.cancel(_rateTextAlpha.gameObject);
        LeanTween.alphaCanvas(_rateTextAlpha, 0, 1f).setEaseOutExpo().setOnComplete(() =>
        {
            _rateTextAlpha.alpha = 0;
        });

    }
}
