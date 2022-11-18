using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PermanentUI : MonoBehaviour
{
    private int _cherryCount = 0;
    [SerializeField]private TMP_Text CherryScore;

    public static PermanentUI inst;

    public int CherryCount { get => _cherryCount; set => _cherryCount = value; }

    private void Awake() {
        if(inst == null){
            inst = this;
        }else{
            Destroy(gameObject);
        }
    }

    
}
