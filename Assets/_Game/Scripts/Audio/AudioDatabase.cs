using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Game/Audio/Audio Database")]
public sealed class AudioDatabase : ScriptableObject
{
    private const string LogPrefix = "[AudioDatabase]";

    [Header("Music")]
    [SerializeField] private List<AudioClipDefinition> _musicClips = new();
    
    [Header("Ambiance")]
    [SerializeField] private List<AudioClipDefinition> _ambianceClips = new();

    [Header("SFX")]
    [SerializeField] private List<AudioClipDefinition> _sfxClips = new();
    
    [Header("Voice")]
    [SerializeField] private List<AudioClipDefinition> _voiceClips = new();

    public bool TryGetMusic(string id, out AudioClipDefinition clipDefinition)
    {
        return TryGetClip(_musicClips, id, out clipDefinition);
    }
    
    public bool TryGetAmbiance(string id, out AudioClipDefinition clipDefinition)
    {
        return TryGetClip(_ambianceClips, id, out clipDefinition);
    }

    public bool TryGetSfx(string id, out AudioClipDefinition clipDefinition)
    {
        return TryGetClip(_sfxClips, id, out clipDefinition);
    }
    
    public bool TryGetVoice(string id, out AudioClipDefinition clipDefinition)
    {
        return TryGetClip(_voiceClips, id, out clipDefinition);
    }

    private static bool TryGetClip(
        IReadOnlyList<AudioClipDefinition> clips,
        string id,
        out AudioClipDefinition clipDefinition)
    {
        clipDefinition = null;

        if (clips == null || string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        for (int i = 0; i < clips.Count; i++)
        {
            AudioClipDefinition candidate = clips[i];

            if (candidate != null && candidate.Id == id)
            {
                clipDefinition = candidate;
                return true;
            }
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ValidateClips(_musicClips, "music");
        ValidateClips(_ambianceClips, "ambiance");
        ValidateClips(_sfxClips, "sfx");
    }

    private void ValidateClips(List<AudioClipDefinition> clips, string category)
    {
        HashSet<string> ids = new();

        if (clips == null)
        {
            return;
        }

        foreach (AudioClipDefinition clipDefinition in clips)
        {
            if (clipDefinition == null)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(clipDefinition.Id))
            {
                Debug.LogWarning($"{LogPrefix} ID audio vide dans {category}.", this);
                continue;
            }

            if (!ids.Add(clipDefinition.Id))
            {
                Debug.LogWarning($"{LogPrefix} ID audio dupliqu� dans {category} : {clipDefinition.Id}", this);
            }
        }
    }
#endif
}