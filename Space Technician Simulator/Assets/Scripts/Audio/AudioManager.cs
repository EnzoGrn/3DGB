using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager _Instance;

    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null)
                return null;
            return _Instance;
        }
    }

    [Header("Audio Sources")]

    [SerializeField]
    private AudioSource _MusicSource;

    [SerializeField]
    private AudioSource _SFXSource;

    private void Awake()
    {
        if (_Instance == null) {
            _Instance = this;

            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip, Transform transform = null, float volume = 1f)
    {
        if (clip == null)
            return;

        if (transform != null) {
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        } else {
            _SFXSource.PlayOneShot(clip, volume);
        }
    }

    public bool IsPlayingSFX(AudioClip clip)
    {
        return _SFXSource.clip == clip;
    }
}
