using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
  [SerializeField]
  private float strafeDistance;

  protected override void move() {
    if (Vector3.Distance(transform.position, player.transform.position) > strafeDistance)
      base.move();
  }
}