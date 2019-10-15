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
    public float speedMultiplier = 0.5f;
    public float maxSpeed = 100f;
    private float adjustedSpeed = 10f;

    public float beamPauseTime = 1f;

    public int reflectionCount = 5;
    private int originalReflectionCount = 0;
    private int maxReflectionCount;
    public float maxBeamLength = 20f;
    private bool fired = false;
    public BeamType beamType = BeamType.radiant;

    public Vector3 playerPosition = Vector3.zero; // get this from the player controller
    private LineRenderer trailLine;

    private void Start () {
        maxReflectionCount = reflectionCount;
        adjustedSpeed = speed;
    }

    void Update(){
        if(Input.GetKeyDown("a") && !fired){
            ActivateBeam();
        }
    }

    public void ActivateBeam(){
        reflectionPoints.Insert(0 , transform.position);
        LaunchBeam(transform.position + this.transform.forward * 0.75f , this.transform.forward, reflectionCount);    
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position , 1f);
        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f , transform.forward , reflectionCount);
    }
    
    public virtual void DrawPredictedReflectionPattern(Vector3 position , Vector3 direction , int reflectionsRemaining){
        if(!fired){
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
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireSphere(hit.point , 1f);
                            break;
                    default:
                        Debug.Log("Default Light Hit, is there something missing?");
                        break;
                }
                }
                else if(hit.rigidbody != null && transform.tag == "Glass"){ // Glass Exception , used for puzzles
                    reflectionsRemaining += 1;
                }
                else{   // Path Ending Stopper
                    position = hit.point;
                    reflectionsRemaining = 0;
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(position , 1f);
                }
            }
            else{   // No-Hit Path Stopper
                position += direction * maxBeamLength;
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(position , 1f);
            }
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(startPos , position);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(position , 0.25f);    // hitpoint

            if(hit.rigidbody != null && hit.transform.tag == "Reflectable" && reflectionsRemaining > 0)
                DrawPredictedReflectionPattern(position , direction , reflectionsRemaining - 1);
        }
    }

    //Version 2 to match up better with the GUI
    public virtual void LaunchBeam(Vector3 position , Vector3 direction , int reflectionsRemaining){
        if(reflectionsRemaining <= 0){
            Debug.Log("Starting to Fire Beam");
            StartCoroutine(RenderBeam());
            return;
        }
        else Debug.Log(reflectionsRemaining);

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
                        break;
                   default:
                    break;
               }
            }
            else if(hit.rigidbody != null && transform.tag == "Glass"){ // Glass Exception , used for puzzles
                reflectionsRemaining += 1;
            }
            else{   // Path Ending Stopper
                position = hit.point;
                reflectionsRemaining = 0;
            }
        }
        else{   // No-Hit Path Stopper
             position += direction * maxBeamLength;
        }

        if(hit.rigidbody != null && reflectionsRemaining >= 0)
        {
            reflectionPoints.Add(position);
            if(hit.transform.tag == "Reflectable"){ // Reflectable Check
                if(hit.transform.GetComponent<ReflectorNode>().nodeType != ReflectorNode.NodeType.Goal)
                    LaunchBeam(position , direction , reflectionsRemaining - 1);
                else{ // is it a Goal?
                    reflectionsRemaining = 0;
                    LaunchBeam(position , direction , 0);
                }
            }
            else{   // Untagged Object Exception
                Debug.Log("Untagged Object hit, no reflection");
                reflectionsRemaining = 0;
                LaunchBeam(position , direction , reflectionsRemaining);
            }
        }
        else{ // No Object Hit Exception
            Debug.Log("No Hit Found at Point");
            Debug.Log("Target: " + position);
            reflectionPoints.Add(position);
            reflectionsRemaining = 0;
            LaunchBeam(position , direction , reflectionsRemaining);
        }
    }

    private void CalcSpeed(){
        float adjustedMultiplier = speedMultiplier + (speedMultiplier * speedMultiplier);
        adjustedSpeed = Mathf.Clamp(adjustedSpeed + (adjustedSpeed * adjustedMultiplier) , 0 , maxSpeed);
    }

    public virtual IEnumerator RenderBeam(){
        fired = true;
        int currentIndex = 0;
        while(currentIndex != reflectionPoints.Count){
            CalcSpeed();
            transform.position = Vector3.MoveTowards(transform.position , reflectionPoints[currentIndex] , Time.deltaTime * adjustedSpeed);
            if(transform.position == reflectionPoints[currentIndex])
            {
                currentIndex++;
                adjustedSpeed = speed;
                yield return new WaitForSeconds(beamPauseTime);
            }
            yield return null;
        }

        reflectionPoints.Clear();
        yield return new WaitForSeconds(beamPauseTime);
        transform.position = playerPosition;
        fired = false;
    }

    private void OnCollisionEnter(Collision other){
        //Debug.Log("Collision!");
        if(other.transform.tag == "Reflectable"){
            ReflectorNode node = other.transform.GetComponent<ReflectorNode>();
            if(node.nodeType == ReflectorNode.NodeType.Goal)
                node.GetComponent<NodeGoal>().Activate();
        }
    }
}