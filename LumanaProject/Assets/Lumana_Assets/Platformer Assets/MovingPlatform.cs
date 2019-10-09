using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    public WayPointGrid moveGrid;
    public bool isActive = false;
    public bool useSensor = false;
    public List<Rigidbody> rbs = new List<Rigidbody>();
    private Vector3 lastPos;
    private Rigidbody rb;

    public float speed = 1f;

    void Start(){
       lastPos = transform.position;
       rb = GetComponent<Rigidbody>();
        if(useSensor)
        foreach(MovingPlatformSensor sensor in GetComponentsInChildren<MovingPlatformSensor>()){
            sensor.platform = this;
       }
    }
    
    void LateUpdate(){
        if(rbs.Count > 0)
        for(int i = 0; i < rbs.Count; i++){
            Rigidbody rb = rbs[i];
            Vector3 velocity = (transform.position - lastPos);
            rb.transform.Translate(velocity , transform);
        }
        lastPos = transform.position;
    }

    void OnCollisionEnter(Collision col){
            if(useSensor) return;
            Rigidbody rb = col.collider.GetComponent<Rigidbody>();
            if(rb != null)
                rbs.Add(rb);
        
    }

    void OnCollisionExit(Collision col){
        if(useSensor) return;       
        Rigidbody rb = col.collider.GetComponent<Rigidbody>();
        if(rb != null)
            rbs.Remove(rb);
        
    }

    public void AddToPlatform(Rigidbody rb){
        if(!rbs.Contains(rb))
            rbs.Add(rb);
    }

    public void RemoveFromPlatform(Rigidbody rb){
        if(rbs.Contains(rb))
            rbs.Remove(rb);
    }

    public void MovePlatform(){
        if(isActive || (!isActive && transform.position != moveGrid.WayPointAtIndex(moveGrid.wayPointIndex))){
            Vector3 target = moveGrid.WayPointAtIndex(moveGrid.wayPointIndex);
            Vector3 movement = Vector3.MoveTowards(transform.position , target , Time.deltaTime * speed);
            rb.MovePosition(movement);
            if(transform.position == target){
                    moveGrid.NextPoint();
            }
        }
    }
    

    void Update(){
        MovePlatform();
    }
}
