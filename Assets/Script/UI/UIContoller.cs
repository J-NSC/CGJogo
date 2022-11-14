using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIContoller : MonoBehaviour
{
    [SerializeField]private TMP_Text CherryCount;
    private PlayerCollider player;


    void Start()
    {
        player = FindObjectOfType<PlayerCollider>();
    }

    void Update()
    {
        CherryCount.text = player.cherry.ToString();
    }
}
