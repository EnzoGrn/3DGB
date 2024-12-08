using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager _Instance;

    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null)
                Debug.LogError("AudioManager doesn't exists.");
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

    public void PlaySFX(AudioClip clip, Transform transform = null)
    {
        if (clip == null)
            return;

        if (transform != null) {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        } else {
            _SFXSource.PlayOneShot(clip);
        }
    }
}
