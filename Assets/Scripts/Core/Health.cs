using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

[System.Serializable]
public class HealthChanged : UnityEvent<float, float>
{

}

//[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public const float minHealth = 0f;
    public float currentHealth;
    public float maxHealth = 100f;
    public HealthChanged OnHealthChanged = new HealthChanged();
    public Pawn machinePawnRef;

    void Start()
    {
        machinePawnRef = GetComponent<Pawn>();

        if (machinePawnRef == null)
        {
            machinePawnRef = GetComponentInParent<Pawn>();
        }

        ResetHealth();

    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float damage, Pawn source)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, minHealth, maxHealth);
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        Debug.Log(source.name + " did " + damage + " damage to " + gameObject.name);
        Debug.Log(source.name + " did " + damage + " damage to " + gameObject.name);
        AudioManager.Instance.audioSource.PlayOneShot(AudioManager.Instance.sfxMachineHurt);
        if (Mathf.Approximately(currentHealth, minHealth))
        {

            Die(machinePawnRef);
        }
    }

    public void ApplyHealing(float value)
    {
        if (value < 0)
        {
            Debug.LogWarning("attempted to heal fort negetive amount (bad)");
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth + value, minHealth, maxHealth);
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        machinePawnRef.machineController.addPoints(20);
        GameManager.Instance.currentPlayerHUD.UpdateScore();

    }

    public void ApplyHurt(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth - value, minHealth, maxHealth);
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    private void Die(Pawn machineToDie)
    {

        //machinePawnRef = GetComponent<Pawn>();
        AudioManager.Instance.audioSource.PlayOneShot(AudioManager.Instance.sfxMachineDeath);
        machinePawnRef.machineController.addPoints(20);
        GameManager.Instance.currentPlayerHUD.UpdateScore();

        // Check if machinePawnRef is not null and get player index
        int playerIndex = -1;  // Initialize playerIndex to an invalid value
        if (machinePawnRef != null)
        {
            playerIndex = GameManager.Instance.GetPlayerIndex(machinePawnRef);
        }

        // Remove life if machinePawnRef is not null
        if (machinePawnRef != null)
        {
            machinePawnRef.machineController.removeLife();
            Debug.Log("remove enemy life" + machinePawnRef.machineController.lives);
        }

        // Reward the person who killed if source is not null and machinePawnRef is not null
        if (machineToDie != null && machinePawnRef != null)
        {
            machineToDie.machineController.addPoints(20);
        }

        // Check if playerIndex is valid and if it is a player, try to respawn the player
        if (playerIndex >= 0 && playerIndex < GameManager.Instance.players.Count)
        {
            if (GameManager.Instance.players[playerIndex].lives > 0)
            {
                GameManager.Instance.ResetPlayer(machinePawnRef.machineController);
            }
        }

        if (machineToDie.machineController.GetType() == typeof(PlayerController))
        {
            Debug.Log("player controller killed!");
            machineToDie.machineController.removeLife();
            if (GameManager.Instance.currentPlayerHUD != null) // check if HUD is not null
            {
                GameManager.Instance.currentPlayerHUD.UpdateLives();
            }
            else
            {
                // HUD is null
                Debug.Log("current player hud is null");
            }
            if (machineToDie.machineController.lives >= 0)
            {
                GameManager.Instance.ResetPlayer(machineToDie.machineController);
            }
            else
            {
                //call game over script
                GameManager.Instance.CallGameOver();
                Destroy(gameObject); //When player dies, we need to just remove the gameObject and respawn it.
            }

        }
        else
        {
            Debug.Log("enemy controller killed!");

            GameManager.Instance.currentPlayerHUD.UpdateScore();
            machinePawnRef.machineController.addPoints(20);
            Destroy(gameObject.transform.parent.gameObject); //When enemy dies, we need to kill it's transform as well. 
        }
    }
}
