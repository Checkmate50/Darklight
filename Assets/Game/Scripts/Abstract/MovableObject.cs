using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
  [SerializeField]
  protected float moveSpeed;
  [SerializeField]
  protected float acceleration; // Only used if physics in effect

  protected GameController gameController;
  protected Rigidbody2D rigidBody;

  public void setGameController(GameController gameController) {
    this.gameController = gameController;
    if (gameController.shouldUsePhysics()) {
      rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }
    else {
      Destroy(gameObject.GetComponent<Rigidbody2D>());
    }
  }

  protected void physicsUpdate() {
    if (gameController == null)
      return;
    if (!gameController.shouldUsePhysics())
      return;
    if (rigidBody.velocity.magnitude < .01) // Avoid weird movement crap
      rigidBody.velocity = Vector2.zero;
  }

  protected abstract bool move();
  protected bool knockback(int amount) {
    return false;
  }
}
