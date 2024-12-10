using System.Collections;
using UnityEngine;

public class SASController : MonoBehaviour {

    [Header("SAS Settings")]

    [SerializeField]
    private AreaTriggerDoor[] _Doors; // !< Doors to open

    [SerializeField]
    private ParticleSystem[] _GazParticle;

    [Header("Sequence Settings")]

    [SerializeField]
    private float _SequenceDuration = 3.0f; // !< Duration of the sequence

    [Header("Sound")]

    [SerializeField]
    private AudioClip _SASSound; // !< SAS Sound

    [SerializeField]
    private Transform _SoundPosition; // !< Position of the sound

    public void StartSequence()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(1);

        AudioManager.Instance.PlaySFX(_SASSound, _SoundPosition, 4f);

        foreach (ParticleSystem gaz in _GazParticle)
            gaz.Play();

        yield return new WaitForSeconds(_SequenceDuration / 2);

        foreach (ParticleSystem gaz in _GazParticle)
            gaz.Play();

        yield return new WaitForSeconds(_SequenceDuration / 2);

        yield return new WaitForSeconds(1);

        StopSequence();
    }

    private void StopSequence()
    {
        // Open the door
        foreach (AreaTriggerDoor door in _Doors)
            door.Open();
    }

    public void CloseDoors()
    {
        // Close the door
        foreach (AreaTriggerDoor door in _Doors)
            door.Close(false);
    }
}
