using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    public static GlobalSound Instance;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float globalVolume = 1f;

    public bool isMuted = false;

    private AudioSource oneShotSource;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("Loop SFX")]
    private AudioSource loopSource;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        oneShotSource = gameObject.AddComponent<AudioSource>();
        oneShotSource.playOnAwake = false;

        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true;
        loopSource.playOnAwake = false;
        loopSource.spatialBlend = 0f; // 2D 

        LoadSettings();
        ApplyVolume();
    }

    /* ================= SETTINGS ================= */

    public void SetGlobalVolume(float volume)
    {
        globalVolume = Mathf.Clamp01(volume);
        isMuted = false;

        ApplyVolume();
        SaveSettings();
    }

    public void SetMute(bool mute)
    {
        isMuted = mute;
        ApplyVolume();
        SaveSettings();
    }

    void ApplyVolume()
    {
        AudioListener.volume = isMuted ? 0f : globalVolume;

        if (bgmSource != null)
            bgmSource.mute = isMuted;
    }

    /* ================= PLAY SFX ================= */

    public void PlaySound(AudioClip clip)
    {
        if (clip == null || isMuted) return;
        oneShotSource.PlayOneShot(clip, globalVolume);
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip == null || isMuted) return;
        AudioSource.PlayClipAtPoint(clip, position, globalVolume);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (clip == null || isMuted)
            return;

        if (loopSource.clip == clip && loopSource.isPlaying)
            return;

        loopSource.clip = clip;
        loopSource.volume = 1f; // AudioListener handles master volume
        loopSource.Play();
    }

    public void StopLoop(AudioClip clip = null)
    {
        if (!loopSource.isPlaying)
            return;

        if (clip != null && loopSource.clip != clip)
            return;

        loopSource.Stop();
        loopSource.clip = null;
    }


    /* ================= SAVE / LOAD ================= */

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("GlobalVolume", globalVolume);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }

    void LoadSettings()
    {
        globalVolume = PlayerPrefs.GetFloat("GlobalVolume", 1f);
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
    }

    public void PlayBGM(AudioClip clip, bool restart = false)
    {
        if (clip == null)
            return;

        if (bgmSource.clip == clip && bgmSource.isPlaying && !restart)
            return;

        bgmSource.clip = clip;
        bgmSource.volume = 1f; // listener handles master volume
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        if (bgmSource.clip != null)
            bgmSource.UnPause();
    }
}
