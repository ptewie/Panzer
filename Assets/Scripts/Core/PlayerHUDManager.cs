using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    private Controller controller;
    private int playerIndex;
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public Image healthBar;
    public Pawn machinePawnRef;
    private void Start()
    {
        machinePawnRef = GetComponent<Pawn>();

        if (machinePawnRef == null)
        {
            machinePawnRef = GetComponentInParent<Pawn>();
        }

        controller = GetComponentInParent<Controller>();
        Debug.Log("controller: " + controller);

        if (controller != null)
        {
            playerIndex = GameManager.Instance.GetPlayerIndex(controller.ControlledPawn);
            Debug.Log("playerindex: " + playerIndex);
        }

        // Check if scoreText is assigned
        if (scoreText != null)
        {
            Debug.Log("scoreText: " + scoreText);
        }
        else
        {
            Debug.LogError("wee woo wee woo no score text assigned");
        }

        UpdateScore();
        UpdateLives();
        gameObject.GetComponentInParent<Health>().OnHealthChanged.AddListener(UpdateHealthBar);
    }

    public void UpdateScore()
    {
        if (controller != null)
        {
            //get playert index from game manager
            int playerIndex = GameManager.Instance.GetPlayerIndex(controller.ControlledPawn);

            if (playerIndex >= 0 && playerIndex < controller.points)
            {
                scoreText.text = "Score: " + controller.points;
            }
            else
            {
                // if index is out of range (goddamit)
                scoreText.text = "Score: N/A";
            }
        }
    }

    public void UpdateLives()
    {
        if (controller != null)
        {
            livesText.text = "Lives: " + controller.lives;
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}