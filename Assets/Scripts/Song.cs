using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Migrated to new script file as scriptable objects require the file name be the same as the class
[System.Serializable]
[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/Make Song", order = 1)]
public class Song : ScriptableObject
{
    public AudioClip song;
    public List<Note> notes = new List<Note>();
    public float noteSpeed;
    public string title;

}
