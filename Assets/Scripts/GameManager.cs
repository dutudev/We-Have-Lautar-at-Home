using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Song currentSong;
    [SerializeField] private int score;
    public static GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int value)
    {
        if (value == 0)
        {
            UIManagerGame.instance.UpdateRateText(value);
            return;
        }
        
        score += value;
        UIManagerGame.instance.UpdateRateText(value);
        UIManagerGame.instance.UpdateScoreText(score);
    }
}
