using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Song currentSong;
    [SerializeField] private int score;
    [SerializeField] private string comPort;
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
            DontDestroyOnLoad(gameObject);
            print(SerialPort.GetPortNames().Length);
            if (SerialPort.GetPortNames().Length != 0)
            {
                comPort = SerialPort.GetPortNames()[0]; 
            }

            SceneManager.activeSceneChanged += SceneChanged;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Song GetCurrentSong()
    {
        return currentSong;
    }

    public void SetCurrentSong(Song song)
    {
        currentSong = song;
    }

    public void SetSerialPort(string port)
    {
        comPort = port;
    }

    public string[] GetSerialPorts()
    {
        return SerialPort.GetPortNames();
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

    public int GetScore()
    {
        return score;
    }
    
    private void SceneChanged(Scene current, Scene next)
    {
        if (next.name == "MainMenu")
        {
            currentSong = null;
            score = 0;
        }
    }

    public void StartGame(Song selected)
    {
        currentSong = selected;
    }

    public string GetPortName()
    {
        return comPort;
    }
}
