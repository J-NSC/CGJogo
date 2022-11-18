using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIContoller : MonoBehaviour
{
    [SerializeField]private TMP_Text CherryScore;
    

    private int _CherryCount = 0;

    public int CherryCount { get => _CherryCount; set => _CherryCount = value; }

    void Update()
    {
        CherryScore.text = _CherryCount.ToString();
    }

    public void onExit(){
        Application.Quit();
    }

    public void onReset(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
