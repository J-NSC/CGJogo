using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    private Enemy enemy;
    private UIContoller ui;

    [ SerializeField] private AudioSource cherrySound;

  

    // get and set

    void Start()
    {
        ui = FindObjectOfType<UIContoller>();
        cherrySound = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){

            ui.CherryCount ++;
            cherrySound.Play();
            Destroy(this.gameObject);
        }
    }
}
