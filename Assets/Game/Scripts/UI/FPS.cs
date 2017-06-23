using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
  private Text healthText;
  private float deltaTime;

  void Start() {
    healthText = GetComponent<Text>();
  }

  // Update is called once per frame
  void Update() {
    deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    healthText.text = "FPS: " + (1.0f / deltaTime).ToString("F2");
  }
}
