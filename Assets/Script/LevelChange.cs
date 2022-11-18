using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    [SerializeField] private string levelName;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LoadLevel(){
        SceneManager.LoadScene(levelName);
    }
}
