using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipDefinition", menuName = "Game/Audio/Audio Clip Definition")]
public sealed class AudioClipDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string _id = "audio_id";

    [Header("Clip")]
    [SerializeField] private AudioClip[] _clips;
    
    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float _volume = 1f;
    [SerializeField] private bool _loop = false;
    
    [Header("Loop(Music)")]
    [SerializeField] private float _loopStart = 0.0f;
    [SerializeField] private float _loopEnd = 0.0f;

    public string Id => _id;
    public AudioClip[] Clips => _clips;
    public float Volume => _volume;
    public bool Loop => _loop;
    public float LoopStart => _loopStart;
    public float LoopEnd => _loopEnd;
}