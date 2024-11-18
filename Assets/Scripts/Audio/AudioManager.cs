using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
//Codigo editor de audio en opciones con audiomixer
public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance;
    public List<Sound> musicSound; // Source de musica en loop
    public List<Sound> sfxSound; // Lista de efectos de sonidos (para UI)

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }


    public void Start()
    {
        PlayMusic("Hood");
    }
    public void PlayMusic(string name)
    {
        Sound s = musicSound.Find(x => x.soundName == name);

        if (s == null)
            Debug.Log("Music Sound not found");
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = sfxSound.Find(x => x.soundName == name);

        if (s == null)
            Debug.Log("SFX not found");
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFXMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}
