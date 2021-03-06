using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class VolumeControl : MonoBehaviour
{

    public AudioMixer mixer;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        mixer = FindObjectOfType<SceneChanger>().mixer;
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
