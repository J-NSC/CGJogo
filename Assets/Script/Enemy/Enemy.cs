using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]

public class  Enemy : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private int nexId = 0 ;
    [SerializeField] private int idChangeValue = 1; 

    [SerializeField] private float speed;


    private void Reset() {
        init();
    }

    void init(){
        GetComponent<CapsuleCollider2D>().isTrigger = false;

        GameObject root = new GameObject(name + "_Root");

        root.transform.position = transform.position;

        transform.SetParent(root.transform);

        GameObject wayPoints = new GameObject("WayPoints");

        wayPoints.transform.SetParent(root.transform);
        wayPoints.transform.position = root.transform.position;

        GameObject p1 = new GameObject("Point1");
        GameObject p2 = new GameObject("Point2");

        p1.transform.SetParent(wayPoints.transform);
        p2.transform.SetParent(wayPoints.transform);

        p1.transform.position =  root.transform.position;
        p2.transform.position =  root.transform.position;

        points = new List<Transform>();
        points.Add(p1.transform); 
        points.Add(p2.transform);

    }


    private void FixedUpdate() {
        MoveToNextPoint();
        
    }

    void MoveToNextPoint(){
        Transform goalPoint = points[nexId];

        if(goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        transform.position = Vector2.MoveTowards(transform.position , goalPoint.position , speed );

        if(Vector2.Distance(transform.position, goalPoint.position) < 1f){
            if(nexId == points.Count -1 ){
                idChangeValue -=1;
            }

            if(nexId == 0){
                idChangeValue =1;
            }

            nexId += idChangeValue;

             
        }
        
         
    }
}
