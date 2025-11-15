using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SongEditor : MonoBehaviour
{
    [SerializeField] private AudioSource songAudioSource;
    [SerializeField] private NoteContainerEditor container;
    private float _songStartDspTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        _songStartDspTime = (float)AudioSettings.dspTime + 5f;
        songAudioSource.PlayScheduled(_songStartDspTime);
        //print();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    
    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            HitTrack(0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            HitTrack(1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            HitTrack(2);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HitTrack(3);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveTrack();
        }
    }

    public void HitTrack(int track)
    {
        Note noteCur = new Note();
        noteCur.track = track;
        noteCur.songTime = (float)AudioSettings.dspTime - _songStartDspTime;
        container.notes.Add(noteCur);
    }

    public void SaveTrack()
    {
        string json = JsonUtility.ToJson(container);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/Songs/TempEditor.json", json);
    }
}


[System.Serializable]
public class NoteContainerEditor
{
    public List<Note> notes;
}

[CustomEditor(typeof(SongEditor))]
public class EditorSongInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("make Level"))
        {
            Song song = CreateInstance<Song>();
            song.notes = JsonUtility.FromJson<NoteContainerEditor>(File.ReadAllText(Application.dataPath + "/Songs/TempEditor.json")).notes;
            AssetDatabase.CreateAsset(song, "Assets/Songs/NewSongRENAME.asset");
        }
    }
}