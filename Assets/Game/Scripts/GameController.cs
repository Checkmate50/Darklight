using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{

  [SerializeField]
  private Player player;
  [SerializeField]
  private CameraController gameCamera;
  [SerializeField]
  private Enemy[] enemies;
  [SerializeField]
  private GameObject[] walls;
  [SerializeField]
  private bool usePhysics;

  private Vector3 wallSize;
  private Player playerInstance;

  // Use this for initialization
  void Start() {
    wallSize = walls[0].transform.lossyScale;
    startSetup();
    placeWalls();
    placeEnemies();
  }

  private void startSetup() {
    playerInstance = Instantiate(player, Vector3.zero, Quaternion.identity);
    playerInstance.setGameController(this);
    CameraController cam = Instantiate(gameCamera, new Vector3(0, 0, -10f), Quaternion.identity);
    cam.setPlayer(playerInstance.gameObject);
  }

  private void placeWalls() {
    buildBoundary(Vector3.zero, 20, 15);
    placeWall(new Vector3(0, 4, 0));
    placeWall(new Vector3(0, -4, 0));
  }

  private void placeEnemies() {
    placeEnemy(enemies, new Vector3(-5, 5, 0));
    placeEnemy(enemies, new Vector3(-8, 3, 0));
    placeEnemy(enemies, new Vector3(6, 2, 0));
    placeEnemy(enemies, new Vector3(6, -4, 0));
    placeEnemy(enemies, new Vector3(-5, -6, 0));
  }

  private void placeEnemy(Enemy[] enemyType, Vector3 position) {
    int index = Random.Range(0, enemyType.Length);
    Enemy e = Instantiate(enemyType[index], position, Quaternion.identity);
    e.setPlayer(playerInstance);
    e.setGameController(this);
  }

  private void buildBoundary(Vector3 center, int width, int height) {
    // Builds a square boundary of walls centered on 'center' with a number of boxes on each edge given
    Vector3 placePos = center;
    placePos.y += wallSize.y * height / 2f;
    placePos.x -= wallSize.x * width / 2f;
    // Top
    for (int i = 0; i < width; i++) {
      placeWall(placePos);
      placePos.x += wallSize.x;
    }
    // Right
    for (int i = 0; i < height; i++) {
      placeWall(placePos);
      placePos.y -= wallSize.y;
    }
    // Bottom
    for (int i = 0; i < width; i++) {
      placeWall(placePos);
      placePos.x -= wallSize.x;
    }
    // Left
    for (int i = 0; i < height; i++) {
      placeWall(placePos);
      placePos.y += wallSize.y;
    }
  }

  private void placeWall(Vector3 position) {
    int index = Random.Range(0, walls.Length);
    Instantiate(walls[index], position, Quaternion.identity);
  }

  public void gameOver() {
    SceneManager.LoadScene(0);
  }

  public bool shouldUsePhysics() {
    return usePhysics;
  }

  public Player getPlayer()
  {
    return playerInstance;
  }
}
