using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [Header("Song Variables")]
    [SerializeField] private Song currentSong;
    [Header("Gameobject Variables")]
    [SerializeField] private AudioSource songAudioSource;
    [SerializeField] private GameObject notePrefab, noteParent;
    [SerializeField] private GameObject[] tracksGameObjects;
    [Header("Game Variables")] 
    [SerializeField] private List<NoteObj> notesLive = new List<NoteObj>(); 
    [SerializeField] private float currentSongTime;
    [SerializeField] private float positionNoteSpawnY;

    private int _lastNote = 0;
    private bool _didNotesFinish = false;
    private float _songStartDspTime = -1;
    private List<NoteObj> _notesToRemove = new List<NoteObj>();
    // Start is called before the first frame update
    void Start()
    {
        //Sort note list
        currentSong.notes.Sort((note1, note2) =>
        {
            return note1.songTime.CompareTo(note2.songTime);
        });

        StartSong();
    }

    // Update is called once per frame
    void Update()
    {
        currentSongTime = (float)AudioSettings.dspTime - _songStartDspTime;
        if (!_didNotesFinish)
        {
            CheckForNotesToStart();
        }

        if (notesLive.Count != 0)
        {
            UpdateNotes();
        }

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
    }

    public void StartSong()
    {
        songAudioSource.clip = currentSong.song;
        _songStartDspTime = (float)AudioSettings.dspTime + 0.5f;
        songAudioSource.PlayScheduled(_songStartDspTime);
    }
    

    public void CheckForNotesToStart(){
        while (currentSong.notes[_lastNote].songTime - currentSong.noteSpeed <= currentSongTime )
        {
            Vector2 positionNote = new Vector2(tracksGameObjects[currentSong.notes[_lastNote].track].transform.position.x, positionNoteSpawnY);

            var curNote = Instantiate(notePrefab, positionNote, Quaternion.identity, noteParent.transform);
            NoteObj noteCur = new NoteObj();
            noteCur.note = currentSong.notes[_lastNote];
            noteCur.obj = curNote;
            
            notesLive.Add(noteCur);
            _lastNote++;
            if (_lastNote == currentSong.notes.Count)
            {
                _didNotesFinish = true;
                break;
            }
        }
    }

    public void UpdateNotes()
    {
         _notesToRemove.Clear();
        foreach (var note in notesLive)
        {
            note.progress = (currentSongTime - (note.note.songTime - currentSong.noteSpeed)) / currentSong.noteSpeed;
            note.obj.transform.position = new Vector2(note.obj.transform.position.x, Mathf.LerpUnclamped(positionNoteSpawnY, -4f, note.progress));
            //print(note.progress);   
            if (note.progress >= 1.1f)
            {
                
                _notesToRemove.Add(note);
                //ADD MISS !!
            }
        }
        UpdateRemoveNotes();
        
    }

    public void HitTrack(int track)
    {
        List<NoteObj> trackNotes = new List<NoteObj>();
        foreach (var note in notesLive)
        {
            if (note.note.track == track)
            {
                trackNotes.Add(note);    
            }
        }

        if (trackNotes.Count != 0)
        {
            int score = 0;
            _notesToRemove.Clear();
            // give score based on progress
            if (trackNotes[0].progress >= 0.85 && trackNotes[0].progress <= 1)
            {
                score = 25 + Mathf.FloorToInt(25 * ((trackNotes[0].progress - 0.85f) / 0.15f));
                // ADD SCORE
                _notesToRemove.Add(trackNotes[0]);
            }else if (trackNotes[0].progress > 1)
            {
                score = 10;
                //ADD score
            }
            UpdateRemoveNotes();
            // I mean add score here not up cause more efficient
        }
    }

    public void UpdateRemoveNotes()
    {
        foreach (var note in _notesToRemove)
        {
            Destroy(note.obj); 
            notesLive.Remove(note);
        }
    }
    
}


/*
 To do List
 -on each frame, check if last note time is smaller than (audio dsp time - time)
 -if so spawn note 
 - for each note, lerp them based on their time relative to the audio dsp
 - if their progress is at ~1.2, class them as a miss
 
 */