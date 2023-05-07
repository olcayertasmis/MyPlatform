using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup restartPanel, startPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Slider musicSlider, sfxSlider;

    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        exitButton.onClick.AddListener(GameManager.Instance.QuitGame);
    }

    public void ActivateRestartPanel()
    {
        restartPanel.blocksRaycasts = true;
        restartPanel.DOFade(1, 0.5f);
    }

    public void DeactivateRestartPanel()
    {
        restartPanel.blocksRaycasts = false;
        restartPanel.DOFade(0, 0.5f);
    }

    public void ActivateStartPanel()
    {
        startPanel.blocksRaycasts = true;
        startPanel.DOFade(1, 0.5f);
    }

    public void DeactivateStartPanel()
    {
        startPanel.blocksRaycasts = false;
        startPanel.DOFade(0, 0.5f);
    }

    public void SettingsPanelOpen()
    {
        settingsPanel.SetActive(true);
    }

    public void SettingsPanelClose()
    {
        settingsPanel.SetActive(false);
    }

    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
    }

    public void ToggleSfx()
    {
        SoundManager.Instance.ToggleSfx();
    }

    public void MusicVolume()
    {
        SoundManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SfxVolume()
    {
        SoundManager.Instance.SfxVolume(sfxSlider.value);
    }
}