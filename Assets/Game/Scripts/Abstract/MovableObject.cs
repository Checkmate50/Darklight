using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
  [SerializeField]
  protected float moveSpeed;
  [SerializeField]
  protected float acceleration;

  protected GameController gameController;
  protected Rigidbody2D rigidBody;

  protected virtual void FixedUpdate() {
    float mag = rigidBody.velocity.magnitude;
    if (mag < .01) // Avoid weird movement crap
      rigidBody.velocity = Vector2.zero;
    if (rigidBody.velocity.magnitude > moveSpeed)
      rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
  }

  protected virtual void Update() {
    return;
  }

  public void setGameController(GameController gameController) {
    this.gameController = gameController;
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
  }

  protected abstract bool move();
  protected virtual bool knockback(Vector2 force, Vector2 position) {
    rigidBody.AddForceAtPosition(force, position, ForceMode2D.Impulse);
    return true;
  }

  protected bool slow() {
    if (rigidBody.velocity.magnitude < .01)
      return false;
    rigidBody.AddForce((-rigidBody.velocity.normalized) * acceleration);
    return true;
  }
}
