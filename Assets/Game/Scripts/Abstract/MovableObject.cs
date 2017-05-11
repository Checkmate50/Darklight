using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {
    [SerializeField]
    protected float moveSpeed;
    
    protected abstract void move();
    protected bool knockback(int amount) {
        return false;
    }
}
