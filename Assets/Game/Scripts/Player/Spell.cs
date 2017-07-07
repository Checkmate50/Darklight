using UnityEngine;

public class Spell : MonoBehaviour {

  [SerializeField]
  protected PlayerAttack attack;
  [SerializeField]
  protected int cooldown;
  [SerializeField]
  protected int damage;
  [SerializeField]
  protected int cost;

  protected int attackCD;

  private void Start() {
    attackCD = 0;
  }

  private void Update() {
    if (attackCD > 0)
      attackCD -= 1;
  }

  public bool canAttack(int mana) {
    //Debug.Log(attackCD);
    return cost <= mana && attackCD == 0;
  }

  public int cast(Vector3 location, Vector3 target) {
    return cast(location, target, false);
  }

  public int cast(Vector3 location, Vector3 target, bool inDirection) {
    attackCD = cooldown;
    Projectile p = Instantiate(attack, location, Quaternion.identity);
    p.setTarget(target, inDirection);
    p.setDamage(damage);
    return cost;
  }
}
