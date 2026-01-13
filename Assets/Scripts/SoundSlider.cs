using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SoundSlider : MonoBehaviour
{
    Slider slider;
    public AudioClip clickSound;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        if (GlobalSound.Instance == null)
            return;

        // Set slider without triggering event
        slider.SetValueWithoutNotify(GlobalSound.Instance.globalVolume);

        // Ensure listener volume matches
        AudioListener.volume = GlobalSound.Instance.isMuted
            ? 0f
            : GlobalSound.Instance.globalVolume;

        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        if (GlobalSound.Instance == null)
            return;

        GlobalSound.Instance.SetGlobalVolume(value);
        GlobalSound.Instance.SetMute(false);
    }
}
