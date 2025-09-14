using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _movementAudioSourcePrimary;
    [SerializeField] private AudioSource _movementAudioSourceSecondary;

    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private List<SoundID_e> _associatedIds;


    private Dictionary<SoundID_e, AudioClip> _soundDatabase;

    private SoundID_e _cSoundIdPrimary;
    private SoundID_e _cSoundIdSecondary;

    private void Awake()
    {
        _soundDatabase = new Dictionary<SoundID_e, AudioClip>();

        for (int i = 0; i < _clips.Count; i++)
        {
            _soundDatabase.Add(_associatedIds[i],_clips[i]);
        }
    }




    public void PlayMovementSoundPrimary(SoundID_e soundID, bool loop, bool repeat = true)
    {
        if (repeat == false && _cSoundIdPrimary == soundID && loop == _movementAudioSourcePrimary.loop) return;
        else
        {
            _movementAudioSourcePrimary.Stop();
            _movementAudioSourcePrimary.clip = _soundDatabase[soundID];
            _movementAudioSourcePrimary.loop = loop;
            _movementAudioSourcePrimary.Play(); 

            _cSoundIdPrimary = soundID;
        }
    }

    public void PlayMovementSoundSecondary(SoundID_e soundID, bool loop, bool repeat = true)
    {
        if (repeat == false && _cSoundIdSecondary == soundID && loop == _movementAudioSourceSecondary.loop) return;
        else
        {
            _movementAudioSourceSecondary.Stop();
            _movementAudioSourceSecondary.clip = _soundDatabase[soundID];
            _movementAudioSourceSecondary.loop = loop;
            _movementAudioSourceSecondary.Play();

            _cSoundIdSecondary = soundID;
        }
    }


    public void StopMovementSoundPrimary()
    {
        _movementAudioSourcePrimary.Stop();
        _cSoundIdPrimary = SoundID_e.None; 
    }
    public void StopMovementSoundSecondary()
    {
        _movementAudioSourceSecondary.Stop();
        _cSoundIdSecondary = SoundID_e.None;
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
