using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _movementAudioSource;

    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private List<SoundID_e> _associatedIds;


    private Dictionary<SoundID_e, AudioClip> _soundDatabase;

    private SoundID_e _cSoundId; 

    private void Awake()
    {
        _soundDatabase = new Dictionary<SoundID_e, AudioClip>();

        for (int i = 0; i < _clips.Count; i++)
        {
            _soundDatabase.Add(_associatedIds[i],_clips[i]);
        }
    }




    public void PlayMovementSound(SoundID_e soundID, bool loop)
    {
        if (_cSoundId == soundID && loop == _movementAudioSource.loop) return;
        else
        {
            _movementAudioSource.Stop();
            _movementAudioSource.clip = _soundDatabase[soundID];
            _movementAudioSource.loop = loop;
            _movementAudioSource.Play(); 

            _cSoundId = soundID;
        }
    }

    public void StopMovementSound()
    {
        _movementAudioSource.Stop();
        _cSoundId = SoundID_e.None; 
    }

}
public enum SoundID_e
{
    None = 0,
    Run = 1,
    Jump = 2,
    Land = 3,
    Dash = 4
}
