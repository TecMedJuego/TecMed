using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
// Controlador de UI para opciones (sonido)
public class SettingsUI : MonoBehaviour
{
    
    public Slider musicSlider, sfxSlider;
    public CanvasGroup canvasGroup;
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusicMute();
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFXMute();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
