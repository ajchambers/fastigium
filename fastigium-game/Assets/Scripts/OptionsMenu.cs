using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {

    public AudioMixer mainMixer;
    public AudioMixer musicMixer;

    public void SetVolume (float volume) {
        mainMixer.SetFloat("masterVolume", volume);
    }

    public void SetQuality (int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void Back() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ClearSaveData() {
        Debug.Log("Save data cleared.");
    }

    // public void SetMusic (bool playMusic) {
    //     if (playMusic) {
    //         musicMixer.setFloat("musicVolume", 0.0f);
    //     } else {
    //         musicMixer.setFloat("musicVolume", -80.0f);
    //     }
    // }
}
