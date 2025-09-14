using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _tracks;
    [SerializeField] private AudioSource _source;

    void Awake()
    {
        _source.clip = _tracks[0];
        _source.Play();
        _source.loop = true;
    }


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
