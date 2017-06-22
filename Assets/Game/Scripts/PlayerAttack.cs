using UnityEngine;

public class PlayerAttack : Projectile
{
  protected override void Start() {
    base.Start();
    collisionLayers = (1 << 10) + (1 << 9); // Obstacles and enemies
  }

  protected override void hit(RaycastHit2D target) {
    if (target.collider.tag == "Enemy")
      rayResults[0].collider.gameObject.GetComponent<Enemy>().takeDamage(damage);
  }
}
