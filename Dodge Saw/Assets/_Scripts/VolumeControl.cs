using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class VolumeControl : MonoBehaviour
{

    public AudioMixer mixer;
    public AudioMixer sfxMixer;
    public Slider slider;
    public volumeType audioType;

    // Start is called before the first frame update
    void Start()
    {
        sfxMixer = FindObjectOfType<SceneChanger>().sfxMixer;
        mixer = FindObjectOfType<SceneChanger>().mixer;
        slider = GetComponent<Slider>();
        if(audioType == volumeType.music)
        {
            slider.value = PlayerPrefs.GetFloat("Volume");
        }
        if(audioType == volumeType.sfx)
        {
            slider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    public enum volumeType { music, sfx};
}
