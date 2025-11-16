using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlsTab : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] drumTops;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private SerialPort serial;
    [SerializeField] private TMP_Dropdown dropdown;
    List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
    // Start is called before the first frame update
    private void OnEnable()
    {
        dropdown.ClearOptions();
        options.Clear();
        foreach (var port in GameManager.instance.GetSerialPorts())
        {
            var data = new TMP_Dropdown.OptionData();
            data.text = port;
            print(port);
            options.Add(data);
        }

        dropdown.options = options;
        if (GameManager.instance.GetPortName() != String.Empty)
        {
            serial = new SerialPort(GameManager.instance.GetPortName(), 9600);
            serial.Open();
        }
        
    }

    public void ChangedOption(int index)
    {
        GameManager.instance.SetSerialPort(options[index].text);
        if (serial != null)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }
        }
        if (GameManager.instance.GetPortName() != String.Empty)
        {
            serial = new SerialPort(GameManager.instance.GetPortName(), 9600);
            serial.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        string input = string.Empty;
        if (serial != null)
        {
        

        if (serial.IsOpen && serial.BytesToRead > 0)
        {
            try
            {
                input = serial.ReadLine().Trim();
            }
            catch (System.TimeoutException)
            {

            }
        }
        }
    if (Input.GetKeyDown(KeyCode.S) || input =="0")
        {
            HitTrack(0);
        }

        if (Input.GetKeyDown(KeyCode.D) || input == "1")
        {
            HitTrack(1);
        }

        if (Input.GetKeyDown(KeyCode.J) || input == "2")
        {
            HitTrack(2);
        }

        if (Input.GetKeyDown(KeyCode.K) || input == "3")
        {
            HitTrack(3);
        }
    }

    private void OnDisable()
    {
        if (serial != null)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }
        }
    }

    public void HitTrack(int value)
    {
        particles[value].Play();
        LeanTween.cancel(drumTops[value].gameObject);
        drumTops[value].alpha = 1;
        LeanTween.alphaCanvas(drumTops[value], 0, 0.5f).setEaseOutExpo();
    }
}
