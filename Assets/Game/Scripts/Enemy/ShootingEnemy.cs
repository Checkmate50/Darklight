using UnityEngine;

public class ShootingEnemy : Enemy
{
  [SerializeField]
  private float strafeDistance;

  protected override bool move() {
    if (Vector3.Distance(transform.position, player.transform.position) > strafeDistance)
      return base.move();
    else
      return slow();
  }
}