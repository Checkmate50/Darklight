using UnityEngine;

public class EnemyAttack : Projectile
{
  protected override void Start() {
    base.Start();
    collisionLayers = (1 << 10) + (1 << 8); // Obstacles and player
  }

  protected override void hit(RaycastHit2D target) {
    if (target.collider.tag == "Player")
      target.collider.gameObject.GetComponent<Player>().takeDamage(damage);
  }
}
