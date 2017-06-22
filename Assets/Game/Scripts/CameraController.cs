using UnityEngine;

public class CameraController : MonoBehaviour
{

  //https://unity3d.com/learn/tutorials/projects/2d-ufo-tutorial/following-player-camera

  private GameObject player = null;
  private Vector3 offset;
	
  public void setPlayer(GameObject player) {
    // Should be called after initialization
    offset = transform.position - player.transform.position;
    this.player = player;
  }

  // LateUpdate is called at the end of the frame
  void LateUpdate() {
    if (player != null)
      transform.position = player.transform.position + offset;
  }
}
