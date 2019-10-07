using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ReflectorNode : MonoBehaviour
{
    public enum NodeType{
        Mirror,
        Torch,
        Node,
        Goal
    };

    public NodeType nodeType = NodeType.Mirror; 
}
