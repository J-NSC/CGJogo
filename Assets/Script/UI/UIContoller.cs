using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIContoller : MonoBehaviour
{
    [SerializeField]private TMP_Text CherryScore;
    
    private Cherry cherry;

    private int _CherryCount = 0;

    public int CherryCount { get => _CherryCount; set => _CherryCount = value; }

    void Start()
    {
        cherry = FindObjectOfType<Cherry>();
        
    }

    void Update()
    {
        CherryScore.text = _CherryCount.ToString();
    }
}
