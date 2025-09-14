using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _tracks;
    [SerializeField] private AudioSource _source;


    public void StartNewTrack(int index)
    {
        _source.clip = _tracks[index];
        _source.Play();
        _source.loop = true; 
    }

    public void CancelCurrentTrack()
    {
        _source.Stop();
    }
}
