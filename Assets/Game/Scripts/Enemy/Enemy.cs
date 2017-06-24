using UnityEngine;

public class Enemy : MovableObject
{

  [SerializeField]
  protected int maxHealth;
  [SerializeField]
  protected EnemyAttack attack;
  [SerializeField]
  protected int cooldown;
  [SerializeField]
  protected float range;
  // Range away from player at which this enemy activates
  [SerializeField]
  protected float activationRange = 10f;

  protected Player player;
  protected bool activated;
  protected int health;
  protected int attackCD;
  protected Vector3 direction;
  protected Vector2 hitbox;

  // Use this for initialization
  protected void Start() {
    activated = false;
    health = maxHealth;
    attackCD = 0;
    hitbox = gameObject.GetComponent<BoxCollider2D>().size;
  }

  protected override void Update() {
    base.Update();
    if (player == null)
      return;
    if (!activated) {
      if (Vector3.Distance(transform.position, player.transform.position) < activationRange)
        activated = true;
      else
        return;
    }
    if (canAttack())
      useAttack();
  }

  // Update is called once per frame
  protected override void FixedUpdate() {
    base.FixedUpdate();
    if (player == null || !activated)
      return;
    move();
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
    useAttack(player.transform.position);
  }

  protected void useAttack(Vector3 target) {
    attackCD = cooldown;
    EnemyAttack a = Instantiate(attack, transform.position - (transform.position - target).normalized * hitbox.y / 2, Quaternion.identity);
    a.setTarget(target);
  }

  protected override bool move() {
    return move(player.transform.position);
  }

  protected bool move(Vector3 location) {
    direction = (location - transform.position).normalized * acceleration;
    rigidBody.AddForce(direction);
    return true;
  }
}