using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointGrid : MonoBehaviour
{
    [HideInInspector]public List<Vector3> wayPoints = new List<Vector3>();
    public bool cyclical = true;
    public bool reverseDirection = false;
    private int wpi;
    public int wayPointIndex{get {return wpi;} }

    void OnDrawGizmos(){
        for(int i = 0; i < transform.childCount; i++){
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.GetChild(i).position , 0.5f);
            if(i > 0 && i != 0){
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.GetChild(i).position , transform.GetChild(i - 1).position);
                if(cyclical && i == transform.childCount - 1)
                    Gizmos.DrawLine(transform.GetChild(i).position , transform.GetChild(0).position);
            }
        }
    }

    void Start(){
        for(int i = 0; i < transform.childCount; i++){
            wayPoints.Add(transform.GetChild(i).position);
        }
    }

    public void NextPoint(){
        if(!reverseDirection){
            wpi++;
            if(wpi > wayPoints.Count - 1)
                if(cyclical)
                    wpi = 0;
                else{
                    wpi--;
                    reverseDirection = true;
                }
        }
        else{
            wpi--;
            if(wpi < 0)
                if(cyclical){
                    wpi = wayPoints.Count - 1;
                }
                else{
                    wpi++;
                    reverseDirection = false;
                }     
        }
    }

    public void ForceGoToPoint(int index){
        if(index > wayPoints.Count - 1)
            index = 0;
        wpi = index;
    }

    public Vector3 WayPointAtIndex(int index){
        if(index > wayPoints.Count - 1)
            index = 0;
        return wayPoints[index];
    }
}
