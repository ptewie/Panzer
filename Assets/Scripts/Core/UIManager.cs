using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject TitleScreenObject;
    public GameObject OptionsMenuObject;
    public GameObject PauseMenuObject;
    public GameObject GameOverObject;
    public GameObject UICamera;
  
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;

    public void UpdatePointsText()
    {
        if (GameManager.Instance.players.Count > 0)
        {
            pointsText.text = GameManager.Instance.players[0].points.ToString();
        }
    }
}

// Start is called before the first frame update
void Start()
    {
     GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
 
       masterVolumeSlider.value = AudioManager.Instance.masterVolume;
       bgmVolumeSlider.value = AudioManager.Instance.bgmVolume;
       sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ToggleUICamera()
    {
       if (UICamera != null)
      {
          UICamera.SetActive(!UICamera.activeInHierarchy);
      }
    }

    public void HideTitleScreenUI()
    {
        TitleScreenObject.SetActive(false);
    }
  
    public void ShowTitleScreenUI()
    {
        TitleScreenObject.SetActive(true);
    }
  
    public void ShowPauseMenu()
    {
        PauseMenuObject.SetActive(true);
    }
  
    public void HidePauseMenu()
    {
        PauseMenuObject.SetActive(false);
    }
  
    public void ShowOptionsMenu()
    {
        OptionsMenuObject.SetActive(true);
    }
  
    public void HideOptionsMenu()
    {
        OptionsMenuObject.SetActive(false);
    }
 
   public void ShowGameOver()
   {
        GameOverObject.SetActive(true);
    }
 
   public void HideGameOver()
   {
        GameOverObject.SetActive(false);
    }

   public void HandleGameStateChanged(GameState previousState, GameState newState)
   {
        AudioManager.Instance.audioSource.PlayOneShot(AudioManager.Instance.sfxMenuButton);
        switch (previousState)
       {
           case GameState.TitleState:
               HideTitleScreenUI();
               break;
           case GameState.OptionsState:
               HideOptionsMenu();
               break;
           case GameState.GameplayState:
               break;
           case GameState.GameOverState:
               HideGameOver();
               break;
           case GameState.Credits:
               // gotta hide credits
               break;
           case GameState.Pause:
               HidePauseMenu();
               break;
       }
       switch (newState)
       {
           case GameState.TitleState:
               ShowTitleScreenUI();
               break;
           case GameState.OptionsState:
               ShowOptionsMenu();
               break;
           case GameState.GameplayState:
               break;
           case GameState.GameOverState:
               ShowGameOver();
               break;
           case GameState.Credits:
               break;
           case GameState.Pause:
               ShowPauseMenu();
               break;
       }
   }
}