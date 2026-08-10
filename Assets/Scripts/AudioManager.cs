using Unity.VisualScripting;
using UnityEngine;
/*
 * Final Project: AudioManager.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the audio manager
 */
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; }

    public AudioSource MusicSource;
    public AudioSource SFXSource;
    
    [SerializeField] private AudioClip backgroundMusic;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }

    /// <summary>
    /// Get the current music and SFX volume values
    /// </summary>
    /// <param name="currentMusicVolume"></param>
    /// <param name="currentSFXVolume"></param>
    public void GetCurrentVolume(out float currentMusicVolume, out float currentSFXVolume)
    {
        currentMusicVolume = MusicSource.volume;
        currentSFXVolume = SFXSource.volume;
    }

    /// <summary>
    /// Plays the background music
    /// </summary>
    private void PlayBackgroundMusic()
    {
        MusicSource.clip = backgroundMusic;
        MusicSource.loop = true;
        MusicSource.Play();
    }

}
