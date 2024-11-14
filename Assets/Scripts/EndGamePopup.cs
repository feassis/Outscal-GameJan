using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endGameMessage;
    [SerializeField] private Button restartGame;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        restartGame.onClick.AddListener(RestartGame);
    }

    public void Open(bool won)
    {
        endGameMessage.text = won ? "You Escaped" : "You Died";
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
