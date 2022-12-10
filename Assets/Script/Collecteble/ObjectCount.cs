using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCount : MonoBehaviour
{
    public static ObjectCount inst ;

    [SerializeField] private List<Transform> cherrys;

    public List<Transform> Cherrys { get => cherrys; set => cherrys = value; }

    private void Awake() {
        if(inst == null){
            inst = this;
        }
    }

    private void Update() {
        Cherrys = getChildren(transform);
    }


    List<Transform> getChildren(Transform parent){

        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent){
            children.Add(child);
        }

        return children;

    }

}
