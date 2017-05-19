using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

  [SerializeField]
  private int damage;
  [SerializeField]
  private float moveSpeed;
  [SerializeField]
  private float spread;

  private Vector3 direction;
  private Collider2D colliderBox;
  private RaycastHit2D[] rayResults = new RaycastHit2D[1];
  private const int collisionLayers = (1 << 10) + (1 << 9); // Obstacles and enemies

  // Use this for initialization
  void Start() {
    colliderBox = gameObject.GetComponent<BoxCollider2D>();
    direction = transform.up;
    direction.x += spread * Random.Range(-1, 1);
    direction.y += spread * Random.Range(-1, 1);
    direction = direction.normalized * moveSpeed;
  }

  // Update is called once per frame
  void Update() {
    if (colliderBox.Raycast(direction, rayResults, 0f, collisionLayers) != 0) {
      if (rayResults[0].collider.tag == "Enemy")
        attack(rayResults[0].collider.gameObject.GetComponent<Enemy>());
      Destroy(gameObject);
    }
    transform.position += direction * Time.deltaTime;
  }

  private void attack(Enemy target) {
    target.takeDamage(damage);
  }
}
