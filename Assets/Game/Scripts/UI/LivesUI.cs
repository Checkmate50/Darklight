using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
  [SerializeField]
  private GameController gameController;
  private Text healthText;

  void Start()
  {
    healthText = GetComponent<Text>();
  }

  // Update is called once per frame
  void Update()
  {
    if (gameController == null)
      return;
    healthText.text = "Lives: " + gameController.getPlayer().getHealth();
  }
}
