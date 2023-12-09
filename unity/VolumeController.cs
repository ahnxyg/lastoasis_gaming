using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Text volumeText = null;

    private void Start()
    {
        LoadValue();
    }
    public void VolumeSlider(float volume)
    {
        float volumeValue = volumeSlider.value;
        volumeText.text = (volumeSlider.value*100).ToString("0.0");

        PlayerPrefs.SetFloat("VolumeValue", volumeValue);

        LoadValue();

    }

    void LoadValue()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}
