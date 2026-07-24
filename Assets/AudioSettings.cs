using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioMixer _audioMixer;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        float masterVolume = PlayerPrefs.GetFloat(MIXER_MASTER, 0.5f);
        float musicVolume = PlayerPrefs.GetFloat(MIXER_MUSIC, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(MIXER_SFX, 0.5f);

        _masterSlider.SetValueWithoutNotify(masterVolume);
        _musicSlider.SetValueWithoutNotify(musicVolume);
        _sfxSlider.SetValueWithoutNotify(sfxVolume);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MIXER_MASTER, _masterSlider.value);
        PlayerPrefs.SetFloat(MIXER_MUSIC, _musicSlider.value);
        PlayerPrefs.SetFloat(MIXER_SFX, _sfxSlider.value);
    }

    private void SetMasterVolume(float value)
    {
        _audioMixer.SetFloat(MIXER_MASTER, ToLogarithmicVolume(value));
    }

    private void SetMusicVolume(float value)
    {
        _audioMixer.SetFloat(MIXER_MUSIC, ToLogarithmicVolume(value));
    }

    private void SetSFXVolume(float value)
    {
        _audioMixer.SetFloat(MIXER_SFX, ToLogarithmicVolume(value));
    }

    private float ToLogarithmicVolume(float sliderValue)
    {
        return Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
    }
}
