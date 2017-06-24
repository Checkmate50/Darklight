using UnityEngine;

public abstract class Projectile : MonoBehaviour {

  [SerializeField]
  protected int damage;
  [SerializeField]
  protected float moveSpeed;
  [SerializeField]
  protected float spread;

  protected Vector3 direction;
  protected Collider2D colliderBox;
  protected RaycastHit2D[] rayResults = new RaycastHit2D[1];
  protected int collisionLayers;
  private float distance;

  // Use this for initialization
  protected virtual void Start () {
    colliderBox = gameObject.GetComponent<BoxCollider2D>();
  }
	
	// Update is called once per frame
	protected virtual void Update () {
    if (colliderBox.Raycast(-direction, rayResults, distance, collisionLayers) != 0) {
      hit(rayResults[0]);
      Destroy(gameObject);
    }
    transform.position += direction * Time.deltaTime;
    distance = (direction * Time.deltaTime).magnitude;  // Check if we hit something on the path
  }

  public void setTarget() {
    // Send projectile in whatever direction you instantiated it in
    setTarget(transform.up + transform.position, false);
  }

  public void setTarget(Vector3 location) {
    // Send projectile towards target location
    setTarget(location, false);
  }

  // Use this for initialization
  public void setTarget(Vector3 location, bool inDirection) {
    // If inDirection is true, shoot in direction rather than towards location
    if (inDirection)
      transform.up = location;
    else
      transform.up = location - transform.position;
    colliderBox = gameObject.GetComponent<BoxCollider2D>();
    direction = transform.up;
    direction.x += spread * Random.Range(-1, 1);
    direction.y += spread * Random.Range(-1, 1);
    direction = direction.normalized * moveSpeed;
  }

  protected abstract void hit(RaycastHit2D target);
}
