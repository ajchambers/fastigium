using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
    public Sound[] songs;
    // public Sound mainMusic;
    // public Sound menuMusic;
    // public Sound cutsceneMusic;
    private SceneController sc;

    public MusicManager mmInstance;
    
    private void Awake() {
        foreach (Sound s in songs) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        sc = FindObjectOfType<SceneController>();
        mmInstance = this;
        
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (MusicChangeIsNeeded()) {
            PlayCorrectMusic();
        }
    }
    private bool MusicChangeIsNeeded() {
        if (sc.currentScene == "") {
            return true;
        }
        if (DetermineSceneType(sc.currentScene) == DetermineSceneType(sc.previousScene)) {
            return false;
        } 
        return true;
    }

    private string DetermineSceneType(string sceneName) {
        if (sceneName == "MainMenu" || sceneName == "OptionsMenu" || sceneName == "") {
            return "Menu";
        } else if (sceneName[0] == 'c') {
            return "Cutscene";
        } else {
            return "Main";
        }
    }
    private void PlayCorrectMusic() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if ((currentSceneName == "MainMenu") || (currentSceneName == "OptionsMenu")) {
            StopAllSongs();
            Play("Menu");
        } else if ((currentSceneName == "EndCutscene")) {
            StopAllSongs();
            Play("Cutscene");
        } else {
            StopAllSongs();
            Play("Main");
        }  
    }

    public void Play(string name) {
        Sound s = Array.Find(songs, songs => songs.name == name);
        if (s == null) {
            Debug.LogWarning("Sound : " + name + " not found.");
            return;
        }
        s.source.Play();
    }

    public void StopAllSongs() {
        foreach (Sound s in songs) {
            s.source.Stop();
        }
    }
}
