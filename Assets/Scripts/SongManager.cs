using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO.Ports;
using Unity.VisualScripting;

public class SongManager : MonoBehaviour
{
    [Header("Song Variables")]
    [SerializeField] private Song currentSong;
    [Header("Gameobject Variables")]
    [SerializeField] private AudioSource songAudioSource;
    [SerializeField] private GameObject notePrefab, noteParent, fireGameObject;
    [SerializeField] private GameObject[] tracksGameObjects, noteFinalGameObjects;
    [Header("Game Variables")] 
    [SerializeField] private List<NoteObj> notesLive = new List<NoteObj>(); 
    [SerializeField] private float currentSongTime;
    [SerializeField] private float positionNoteSpawnY;
    [SerializeField] private Material movingMotif, movingMotif1, fire;

    private int _lastNote = 0, _combo = 0;
    private bool _didNotesFinish = false, _endmenuOpen = false, _returnMenu = false;
    private float _songStartDspTime = -1, _timeLeftEnd;
    private List<NoteObj> _notesToRemove = new List<NoteObj>();

    private SerialPort serial;
    // Start is called before the first frame update
    void Start()
    {
        currentSong = GameManager.instance.GetCurrentSong();
        //Sort note list
        currentSong.notes.Sort((note1, note2) =>
        {
            return note1.songTime.CompareTo(note2.songTime);
        });

        StartSong();
        serial = new SerialPort(GameManager.instance.GetPortName(), 9600);
        serial.Open();
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
        if (_didNotesFinish && !songAudioSource.isPlaying && !_endmenuOpen)
        {
            UIManagerGame.instance.OpenFinalMenu();
        }
        
        if (_didNotesFinish && !songAudioSource.isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            UIManagerGame.instance.ReturnMainMenu();
        }
    }

    public void HandleInput()
    {
        string input = string.Empty;
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
       // print(input);
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

        print(_timeLeftEnd);
        if (Input.GetKey(KeyCode.Escape) && !_returnMenu)
        {
            _timeLeftEnd += Time.deltaTime;
            if (_timeLeftEnd >= 2f)
            {
                _returnMenu = true;
                UIManagerGame.instance.ReturnMainMenu();
                LeanTween.value(gameObject, 0.8f, 0f,0f).setOnUpdate((value) =>
                {
                    songAudioSource.volume = value;
                });
            }
        }
        else
        {
            _timeLeftEnd = 0f;
        }
    }

    public void OnApplicationQuit()
    {
        serial.Close();
    }

    public void StartSong()
    {
        songAudioSource.clip = currentSong.song;
        _songStartDspTime = (float)AudioSettings.dspTime + 3f;
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
                GameManager.instance.UpdateScore(0);
                //ADD MISS !!
                UpdateCombo(-1);
            }
        }
        UpdateRemoveNotes();
        
    }

    public void HitTrack(int track)
    {
        List<NoteObj> trackNotes = new List<NoteObj>();
        LeanTween.cancel(noteFinalGameObjects[track]);
        noteFinalGameObjects[track].transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
        LeanTween.scale(noteFinalGameObjects[track], new Vector3(0.25f, 0.25f, 0.25f), 0.5f).setEaseOutExpo().setOnComplete(
            () =>
            {
                noteFinalGameObjects[track].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            });
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
                score = 20 + Mathf.FloorToInt(30 * ((trackNotes[0].progress - 0.85f) / 0.15f));
                // ADD SCORE
                
                if (score >= 45)
                {
                    UpdateCombo(2);
                    /*
                    LeanTween.cancel(gameObject);
                    LeanTween.value(gameObject, 0, 1, 1f).setEaseOutExpo().setOnUpdate((value) =>
                    {
                        movingMotif.SetFloat("_speed", Mathf.Lerp(0.5f, 0.3f, value));
                        movingMotif1.SetFloat("_speed", Mathf.Lerp(-0.5f, -0.2f, value));
                    }).setOnComplete(() =>
                    {
                        movingMotif.SetFloat("_speed", 0.3f);
                        movingMotif1.SetFloat("_speed", -0.2f);
                    });*/
                }else if (score >= 35)
                {
                    UpdateCombo(1);
                }
                else
                {
                    UpdateCombo(-1);
                }
                _notesToRemove.Add(trackNotes[0]);
            }else if (trackNotes[0].progress > 1)
            {
                UpdateCombo(-1);
                score = 10;
                //ADD score
                _notesToRemove.Add(trackNotes[0]);
            }
            UpdateRemoveNotes();
            if (score != 0)
            {
                GameManager.instance.UpdateScore(score); 
            }
            
        }
    }

    public void UpdateCombo(int value)
    {
        float currentLerp = (_combo - 5f) / 10f;
        float time = 1f;
        if (value == -1)
        {
            _combo = 0;
            currentLerp = Mathf.Clamp((_combo - 5f) / 10f, 0f, 1f);
            time = currentLerp*15f;
        }
        else
        {
            _combo += value;
        }
        
        LeanTween.cancel(fireGameObject);
        LeanTween.value(fireGameObject, currentLerp, (_combo - 5f) / 10f, time).setEaseOutExpo().setOnUpdate((float valueLerp) =>
        {
            fireGameObject.transform.localScale = new Vector3(Mathf.Lerp(1, 3.5f, valueLerp), Mathf.Lerp(1, 3.5f, valueLerp), Mathf.Lerp(1, 3.5f, valueLerp));
            fire.SetFloat("_size", Mathf.Lerp(0, 3f, valueLerp));
        });
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