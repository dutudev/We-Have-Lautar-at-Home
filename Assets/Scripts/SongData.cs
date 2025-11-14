using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class Note
{ 
    public float songTime;
    public int track; // 0-1-2-3 tracks
}

[System.Serializable]
[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/Make Song", order = 1)]
public class Song : ScriptableObject
{
    public AudioClip song;
    public List<Note> notes = new List<Note>();
    public float noteSpeed;
    public string title;

}