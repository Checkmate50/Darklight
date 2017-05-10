using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private float movementSpeed;
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
    private PlayerAttack attackPrefab;


    private int health;
    private bool moveUp;
    private bool moveDown;
    private bool moveLeft;
    private bool moveRight;
    
    private bool canFire;
    private int fireCD;

    // Movement stuff (no physics!)
    private Vector2 hitbox;
    private BoxCollider2D collisionBox;
    private const int collisionLayers = (1 << 10) + (1 << 9); // Obstacles and enemies
    private Vector3 direction;
    private Vector3[] raycastOffsets;
    private float movementOffset;

    public int getHealth() {
        return health;
    }

    public void takeDamage(int amount) {
        health -= amount;
    }

    // Use this for initialization
    void Start() {
        health = maxHealth;
        movementOffset = Time.deltaTime * movementSpeed;
        collisionBox = gameObject.GetComponent<BoxCollider2D>();
        hitbox = collisionBox.size;
        raycastOffsets = new Vector3[4];
        raycastOffsets[0] = new Vector3(0, hitbox.y, 0);
        raycastOffsets[1] = new Vector3(0, -hitbox.y, 0);
        raycastOffsets[2] = new Vector3(hitbox.x, 0, 0);
        raycastOffsets[3] = new Vector3(hitbox.x, 0, 0);
    }

    // Update is called once per frame
    void Update() {
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

    private void move() {
        if (moveLeft || moveRight) {
            direction = Vector3.zero;
            if (moveLeft)
                direction.x = -movementOffset;
            if (moveRight)
                direction.x = movementOffset;
            if (Physics2D.Raycast(transform.position, direction.normalized, hitbox.x + movementOffset, collisionLayers).collider == null &&
                Physics2D.Raycast(transform.position + raycastOffsets[0], direction.normalized, hitbox.x + movementOffset, collisionLayers).collider == null &&
                Physics2D.Raycast(transform.position + raycastOffsets[1], direction.normalized, hitbox.x + movementOffset, collisionLayers).collider == null)
                transform.position += direction;
        }

        // Now move along the y-axis separately so collisions work out
        if (moveUp || moveDown) {
            direction = Vector3.zero;
            if (moveUp)
                direction.y = movementOffset;
            if (moveDown)
                direction.y = -movementOffset;
            if (Physics2D.Raycast(transform.position, direction.normalized, hitbox.y + movementOffset, collisionLayers).collider == null &&
                Physics2D.Raycast(transform.position + raycastOffsets[2], direction.normalized, hitbox.y + movementOffset, collisionLayers).collider == null &&
                Physics2D.Raycast(transform.position + raycastOffsets[3], direction.normalized, hitbox.y + movementOffset, collisionLayers).collider == null)
                transform.position += direction;
        }
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
        if (fireCD > 0) {
            fireCD--;
            return false;
        }
        return Input.GetMouseButton(0);
    }

    private void fireWeapon() {
        fireCD = cooldown;
        Instantiate(attackPrefab, transform.position + transform.up * .5f, transform.rotation);
    }
}
