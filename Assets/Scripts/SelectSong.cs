using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SelectSong : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private List<Song> availableSongs = new List<Song>();
    [SerializeField] private GameObject vinylPrefab, vinylParent;
    [SerializeField] private float rotationSpeed;

    private List<GameObject> vinyls = new List<GameObject>();
    private float _rotation;
    private int _currentSongIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        int posY = 0;
        foreach (var song in availableSongs)
        {
            var vinyl = Instantiate(vinylPrefab, new Vector3(0, 0, 0), quaternion.identity, vinylParent.transform);
            vinyls.Add(vinyl); 
            vinyl.transform.localPosition = new Vector3(0, posY, 0);
            posY += -800;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _rotation += Time.deltaTime * rotationSpeed;
        UpdateVinyls();
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_currentSongIndex == 0)
            {
                _currentSongIndex = availableSongs.Count;
            }
            LeanTween.cancel(vinylParent);
            _currentSongIndex--;
            LeanTween.moveLocalY(vinylParent, _currentSongIndex * 800, 1f).setEaseOutExpo();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_currentSongIndex == availableSongs.Count - 1)
            {
                _currentSongIndex = -1;
            }
            LeanTween.cancel(vinylParent);
            _currentSongIndex++;
            LeanTween.moveLocalY(vinylParent, _currentSongIndex * 800, 1f).setEaseOutExpo();
        }
    }

    public void UpdateVinyls()
    {
        foreach (var vinyl in vinyls)
        {
            vinyl.transform.localPosition = new Vector3(-1205 + (1205 - 325) * curve.Evaluate((vinyl.transform.position.y + 7.4f) / 14.8f), vinyl.transform.localPosition.y, 0);
            vinyl.transform.rotation = Quaternion.Euler(0, 0, _rotation);
        }
    }
}
