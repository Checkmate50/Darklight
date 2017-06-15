using UnityEngine;
using System.Collections;

public class Player : MovableObject
{

  [SerializeField]
  private int maxHealth;
  [SerializeField]
  private int cooldown;

  [SerializeField]
  private string up;
  [SerializeField]
  private string down;
  [SerializeField]
  private string left;
  [SerializeField]
  private string right;
  [SerializeField]
  private PlayerAttack attack;


  private int health;
  private bool moveUp;
  private bool moveDown;
  private bool moveLeft;
  private bool moveRight;

  private bool canAttack;
  private int attackCD;

  // Movement stuff (no physics!)
  private Vector2 hitbox;
  private const int collisionLayers = (1 << 10) + (1 << 9); // Obstacles and enemies
  private Vector3 direction;
  private Vector3[] raycastOffsets;
  private float movementOffset;

  public int getHealth() {
    return health;
  }

  public void takeDamage(int amount) {
    health -= amount;
    if (health <= 0) {
      gameController.gameOver();
    }
  }

  // Use this for initialization
  void Start() {
    health = maxHealth;
    hitbox = gameObject.GetComponent<BoxCollider2D>().size;
    raycastOffsets = new Vector3[4];
    raycastOffsets[0] = new Vector3(0, hitbox.y, 0);
    raycastOffsets[1] = new Vector3(0, -hitbox.y, 0);
    raycastOffsets[2] = new Vector3(hitbox.x, 0, 0);
    raycastOffsets[3] = new Vector3(hitbox.x, 0, 0);
  }

  // Update is called once per frame
  void Update() {
    physicsUpdate();
    checkMovement();
    move();
    pointToMouse();
    if (checkFire())
      fireWeapon();
  }

  private void checkMovement() {
    if (Input.GetKeyDown(up))
      moveUp = true;
    else if (Input.GetKeyUp(up))
      moveUp = false;
    if (Input.GetKeyDown(down))
      moveDown = true;
    else if (Input.GetKeyUp(down))
      moveDown = false;
    if (Input.GetKeyDown(left))
      moveLeft = true;
    else if (Input.GetKeyUp(left))
      moveLeft = false;
    if (Input.GetKeyDown(right))
      moveRight = true;
    else if (Input.GetKeyUp(right))
      moveRight = false;
  }

  protected override bool move() {
    if (gameController.shouldUsePhysics())
      return physicsMove();
    bool toReturn = false;
    movementOffset = Time.deltaTime * moveSpeed;
    if (moveLeft || moveRight) {
      direction = Vector3.zero;
      if (moveLeft)
        direction.x = -1;
      if (moveRight)
        direction.x = 1;
      if (Physics2D.Raycast(transform.position, direction.normalized, hitbox.x + movementOffset, collisionLayers).collider == null &&
          Physics2D.Raycast(transform.position + raycastOffsets[0], direction.normalized, hitbox.x + movementOffset, collisionLayers).collider == null &&
          Physics2D.Raycast(transform.position + raycastOffsets[1], direction.normalized, hitbox.x + movementOffset, collisionLayers).collider == null) {
        transform.position += direction * movementOffset;
        toReturn = true;
      }
    }

    // Now move along the y-axis separately so collisions work out
    if (moveUp || moveDown) {
      direction = Vector3.zero;
      if (moveUp)
        direction.y = 1;
      if (moveDown)
        direction.y = -1;
      if (Physics2D.Raycast(transform.position, direction, hitbox.y + movementOffset, collisionLayers).collider == null &&
          Physics2D.Raycast(transform.position + raycastOffsets[2], direction, hitbox.y + movementOffset, collisionLayers).collider == null &&
          Physics2D.Raycast(transform.position + raycastOffsets[3], direction, hitbox.y + movementOffset, collisionLayers).collider == null) {
        transform.position += direction * movementOffset;
        toReturn = true;
      }
    }
    return toReturn;
  }

  private bool physicsMove() {
    Vector2 v = Vector2.zero;
    movementOffset = moveSpeed;
    if (moveUp)
      v.y += movementOffset;
    if (moveDown)
      v.y -= movementOffset;
    if (moveLeft)
      v.x -= movementOffset;
    if (moveRight)
      v.x += movementOffset;
    rigidBody.velocity = v;
    return true;
  }

  private void pointToMouse() {
    //http://answers.unity3d.com/questions/10615/rotate-objectweapon-towards-mouse-cursor-2d.html
    Vector3 mousePos, objectPos;
    float angle;
    mousePos = Input.mousePosition;
    mousePos.z = 5.23f; //The distance between the camera and object
    objectPos = Camera.main.WorldToScreenPoint(transform.position);
    mousePos.x = mousePos.x - objectPos.x;
    mousePos.y = mousePos.y - objectPos.y;
    angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
  }

  private bool checkFire() {
    // Determines if we fire the weapon
    if (attackCD > 0) {
      attackCD--;
      return false;
    }
    return Input.GetMouseButton(0);
  }

  private void fireWeapon() {
    attackCD = cooldown;
    Instantiate(attack, transform.position + transform.up * hitbox.y, transform.rotation);
  }
}
