using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager _Instance;

    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null) {
                GameObject go = new("AudioManager");

                _Instance = go.AddComponent<AudioManager>();
            }
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

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            _SFXSource.PlayOneShot(clip);
    }
}
