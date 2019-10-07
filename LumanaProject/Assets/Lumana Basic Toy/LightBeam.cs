using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Requires the tag "Reflectable"

public class LightBeam : MonoBehaviour {

    public List<Vector3> reflectionPoints = new List<Vector3>();
    public enum BeamType {
        radiant
    };

    public float speed = 10f;
    public int reflectionCount = 5;
    private int maxReflectionCount;
    public float maxBeamLength = 20f;
    private bool fired = false;
    public BeamType beamType = BeamType.radiant;

    private Vector3 playerPosition = Vector3.zero; // get this from the player controller
    private LineRenderer trailLine;

    private void Start () {
        maxReflectionCount = reflectionCount;
    }

    void Update(){
        if(Input.GetKeyDown("a")) ActivateBeam();
    }

    public void ActivateBeam(){
        reflectionPoints.Insert(0 , transform.position);
        LaunchBeam(transform.position + this.transform.forward * 0.75f , this.transform.forward, reflectionCount);    
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position , 1f);
        // change the 0.75f to something better, it will go through walls until adjusted
        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f , transform.forward , reflectionCount);
    }
    
    public virtual void DrawPredictedReflectionPattern(Vector3 position , Vector3 direction , int reflectionsRemaining){
        if(reflectionsRemaining == 0) return;

        Vector3 startPos = position;
        Ray ray = new Ray(position , direction);
        RaycastHit hit;
        if(Physics.Raycast(ray , out hit , maxBeamLength)){
           if(hit.rigidbody != null && hit.transform.tag == "Reflectable"){
                switch(hit.transform.GetComponent<ReflectorNode>().nodeType){
                   case ReflectorNode.NodeType.Mirror:
                        direction = Vector3.Reflect(direction , hit.normal);
                        position = hit.point;
                        break;
                    case ReflectorNode.NodeType.Torch:
                        direction = Vector3.up;
                        position = hit.transform.Find("ReflectorPoint").position;
                        break;
                    case ReflectorNode.NodeType.Node:
                        Transform hitPoint = hit.transform.Find("ReflectorPoint");
                        direction = hitPoint.forward;
                        position = hitPoint.position;
                        break;
                    case ReflectorNode.NodeType.Goal:
                        position = hit.point;
                        reflectionsRemaining = 0;
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(hit.point , 1f);
                        break;
                   default:
                    break;
               }
            }
            else if(hit.rigidbody != null && transform.tag == "Glass"){
                reflectionsRemaining += 1;
            }
        }
        else{
             position += direction * maxBeamLength;
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireSphere(position , 3f);
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPos , position);
        Gizmos.DrawWireSphere(position , 0.25f);    // hitpoint

        if(hit.rigidbody != null && hit.transform.tag == "Reflectable")
            DrawPredictedReflectionPattern(position , direction , reflectionsRemaining - 1);
    }

    // the initial position needs to be set in the update method
    // so that the beam knows the starting position
    public virtual void LaunchBeam(Vector3 position , Vector3 direction , int reflectionsRemaining){
        if(reflectionsRemaining == 0){
            if(!fired)
                StartCoroutine(RenderBeam());
            return;
        };

        Vector3 startPos = position;
        Ray ray = new Ray(position , direction);
        RaycastHit hit;
        if(Physics.Raycast(ray , out hit , maxBeamLength)){
           if(hit.rigidbody != null && hit.transform.tag == "Reflectable"){
                switch(hit.transform.GetComponent<ReflectorNode>().nodeType){
                   case ReflectorNode.NodeType.Mirror:
                        direction = Vector3.Reflect(direction , hit.normal);
                        position = hit.point;
                        break;
                    case ReflectorNode.NodeType.Torch:
                        direction = Vector3.up;
                        position = hit.transform.Find("ReflectorPoint").position;
                        break;
                    case ReflectorNode.NodeType.Node:
                        Transform hitPoint = hit.transform.Find("ReflectorPoint");
                        direction = hitPoint.forward;
                        position = hitPoint.position; 
                        break;
                    case ReflectorNode.NodeType.Goal:
                        Debug.Log(hit.point);
                        position = hit.point;
                        break;
                   default:
                    break;
               }
            }
        }
        else {
            position += direction * maxBeamLength;
            reflectionPoints.Add(position);
            StartCoroutine(RenderBeam());
        }
        

        if(hit.rigidbody != null && hit.transform.tag == "Reflectable")
        {
            reflectionPoints.Add(position);
            LaunchBeam(position , direction , reflectionsRemaining - 1);
        }
    }

    private void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Glass")
            Physics.IgnoreCollision(transform.GetComponent<Collider>() , col.transform.GetComponent<Collider>());
        else if(col.gameObject.tag != "Reflectable")
            Destroy(gameObject);
        
    }

    public virtual IEnumerator RenderBeam(){
        // TODO: adjust the speed algorythm to make the shot look crispy
        fired = true;
        int currentIndex = 0;
        while(currentIndex != reflectionPoints.Count){
            Debug.Log("Target: " + reflectionPoints[currentIndex]);
            transform.position = Vector3.MoveTowards(transform.position , reflectionPoints[currentIndex] , Time.deltaTime * speed);
            if(transform.position == reflectionPoints[currentIndex])
            {
                currentIndex++;
                Debug.Log("New Index: " + currentIndex);
            }
            yield return null;
        }

        reflectionPoints.Clear();
        Debug.Log("Ending");
        yield return new WaitForSeconds(2.5f);
        transform.position = playerPosition;
        fired = false;
    }
}