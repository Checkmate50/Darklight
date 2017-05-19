using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovableObject
{

  [SerializeField]
  private int maxHealth;
  [SerializeField]
  private EnemyAttack attack;
  [SerializeField]
  private int cooldown;
  [SerializeField]
  private float range;
  // Range away from player at which this enemy activates
  [SerializeField]
  private float activationRange = 10f;

  protected Player player;
  protected bool activated;
  protected int health;
  protected int attackCD;

  // Movement stuff (no physics!)
  protected float movementOffset;
  private RaycastHit2D[] rayResults = new RaycastHit2D[1];
  protected BoxCollider2D colliderBox;
  protected Vector2 hitbox;
  protected const int collisionLayers = (1 << 10) + (1 << 9) + (1 << 8); // Obstacles, other enemies, and the player
  protected Vector3 direction;

  // Use this for initialization
  protected void Start() {
    activated = false;
    health = maxHealth;
    attackCD = 0;
    colliderBox = gameObject.GetComponent<BoxCollider2D>();
    hitbox = colliderBox.size;
  }

  // Update is called once per frame
  protected void Update() {
    if (player == null)
      return;
    if (!activated) {
      if (Vector3.Distance(transform.position, player.transform.position) < activationRange)
        activated = true;
      else
        return;
    }
    move();
    if (canAttack())
      useAttack();
  }

  public void setPlayer(Player player) {
    this.player = player;
  }

  public void takeDamage(int damage) {
    health -= damage;
    if (health <= 0)
      Destroy(gameObject);
  }

  protected bool canAttack() {
    if (attackCD <= 0 && Vector3.Distance(transform.position, player.transform.position) < range)
      return true;
    attackCD -= 1;
    return false;
  }

  protected void useAttack() {
    attackCD = cooldown;
    Vector3 target = player.transform.position;
    EnemyAttack a = Instantiate(attack, transform.position - (transform.position - target).normalized * hitbox.y / 2, Quaternion.identity);
    a.setTarget(target);
  }

  protected void useAttack(Vector3 target) {
    attackCD = cooldown;
    EnemyAttack a = Instantiate(attack, transform.position - (transform.position - target).normalized * hitbox.y, Quaternion.identity);
    a.setTarget(target);
  }

  protected override void move() {
    move(player.transform.position);
  }

  protected void move(Vector3 location) {
    movementOffset = Time.deltaTime * moveSpeed;
    direction = (location - transform.position).normalized;
    if (colliderBox.Raycast(direction, rayResults, hitbox.y + movementOffset, collisionLayers) == 0)
      transform.position += direction * movementOffset;
  }
}