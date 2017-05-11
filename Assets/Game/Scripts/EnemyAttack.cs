using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField]
    private int damage;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float spread;

    private Vector3 direction;
    private Collider2D colliderBox;
    private RaycastHit2D[] rayResults = new RaycastHit2D[1];
    private const int collisionLayers = (1 << 10) + (1 << 8); // Obstacles and player

    // Use this for initialization
    public void setTarget(Vector3 location) {
        transform.up = location - transform.position;
        colliderBox = gameObject.GetComponent<BoxCollider2D>();
        direction = transform.up;
        direction.x += spread * Random.Range(-1, 1);
        direction.y += spread * Random.Range(-1, 1);
        direction = direction.normalized * movementSpeed;
    }

    // Update is called once per frame
    void Update() {
        if (direction == null)
            return;
        if (colliderBox.Raycast(direction, rayResults, 0f, collisionLayers) != 0) {
            if (rayResults[0].collider.tag == "Player")
                attack(rayResults[0].collider.gameObject.GetComponent<Player>());
            Destroy(gameObject);
        }
        transform.position += direction * Time.deltaTime;
    }

    private void attack(Player player) {
        player.takeDamage(damage);
    }
}
