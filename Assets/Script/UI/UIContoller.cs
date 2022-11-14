using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIContoller : MonoBehaviour
{
    [SerializeField]private TMP_Text CherryCount;
    private Player player;


    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        CherryCount.text = player.cherry.ToString();
    }
}
