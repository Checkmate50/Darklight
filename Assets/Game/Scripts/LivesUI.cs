using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
  [SerializeField]
  private GameController game;
  private Text healthText;

  void Start()
  {
    healthText = GetComponent<Text>();
  }

  // Update is called once per frame
  void Update()
  {
    healthText.text = "Lives: " + game.getPlayer().getHealth();
    Debug.Log(game.getPlayer().getHealth());
  }
}
