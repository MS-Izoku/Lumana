using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable 
{
    public string name;
    public bool collected = false;

    public Collectable CreateCollectable(string name){
        Collectable collectable = new Collectable();
        return collectable;
    }
}
