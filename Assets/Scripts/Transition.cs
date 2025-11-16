using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(25, 25, 25);
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 1f).setEaseOutExpo().setOnComplete(() =>
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
