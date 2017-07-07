using UnityEngine;

public class Casting : MonoBehaviour {

  [SerializeField]
  private string primary;
  [SerializeField]
  private string secondary;
  [SerializeField]
  private string tertiary;
  [SerializeField]
  private string quaternary;
  [SerializeField]
  private Spell[] spells;
  [SerializeField]
  private int maxMana;

  private int mana;
  private Vector2 hitbox;
  private bool[] status;

  private void Start() {
    mana = maxMana;
    hitbox = gameObject.GetComponent<BoxCollider2D>().size;
    status = new bool[4];
    for (int i = 0; i < status.Length; i++) {
      status[i] = false;
      spells[i] = Instantiate(spells[i], transform); //Just assume we have 4 spells for now
    }
  }

  private void Update() {
    checkFire();
    for (int i = 0; i < spells.Length; i++)
      castSpell(i);
  }

  private void checkFire() {
    status[0] = checkKey(primary, status[0]);
    status[1] = checkKey(secondary, status[1]);
    status[2] = checkKey(tertiary, status[2]);
    status[3] = checkKey(quaternary, status[3]);
  }

  private bool checkKey(string key, bool current) {
    if (key == "lmb") {
      if (Input.GetMouseButtonDown(0))
        return true;
      else if (Input.GetMouseButtonUp(0))
        return false;
    }
    else if (key == "rmb") {
      if (Input.GetMouseButtonDown(1))
        return true;
      else if (Input.GetMouseButtonUp(1))
        return false;
    }
    else {
      if (Input.GetKeyDown(key))
        return true;
      else if (Input.GetKeyUp(key))
        return false;
    }
    return current;
  }

  public bool canFire(int spell) {
    // Determines if we fire the weapon
    return spells[spell].canAttack(mana);
  }

  private void castSpell(int spell) {
    if (status[spell] && canFire(spell))
      mana -= spells[spell].cast(transform.position + (transform.up * hitbox.y/2), transform.up, true);
  }
}
