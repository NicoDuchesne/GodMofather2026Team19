using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class AudioManager : MonoBehaviour
{
    private const string LogPrefix = "[AudioManager]";

    [Header("Data")]
    [SerializeField] private AudioDatabase _audioDatabase;


    [Header("Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _ambianceSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _voiceSource;
    [SerializeField] MusicLooper _musicLooper;

    private string _currentMusicId = string.Empty;
    private string _currentAmbianceId = string.Empty;

    public static AudioManager Instance { get; private set; }

    public bool IsMusicPlaying => _musicSource != null && _musicSource.isPlaying;
    public bool IsAmbiancePlaying => _ambianceSource != null && _ambianceSource.isPlaying;
    public string CurrentMusicId => _currentMusicId;
    public string CurrentAmbianceId => _currentAmbianceId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        ValidateReferences();
    }


    //MUSIC
    
    public void PlayMusic(string musicId)
    {
        if (_audioDatabase == null || _musicSource == null)
        {
            return;
        }

        if (!_audioDatabase.TryGetMusic(musicId, out AudioClipDefinition clipDefinition))
        {
            Debug.LogWarning($"{LogPrefix} Musique introuvable : {musicId}");
            return;
        }

        if (clipDefinition.Clips == null)
        {
            return;
        }

        if (_currentMusicId == musicId && _musicSource.isPlaying)
        {
            return;
        }

        _currentMusicId = musicId;

        _musicLooper.SetValues(clipDefinition);
        _musicSource.volume = clipDefinition.Volume;
        _musicLooper.PlayMusic();
    }
    
    
    public async UniTask StopMusicAsync(float endingTime)
    {
        if (_musicSource == null || !IsMusicPlaying)
        {
            return;
        }

        float t = endingTime;
        float start = _musicSource.volume;
        float end = 0.0f;
        float alpha = 1.0f;
        
        while(t > 0.0f)
        {
            alpha = Mathf.Clamp01(t / endingTime);
            _musicSource.volume = Mathf.Clamp01(Mathf.Lerp(end, start, alpha));
            t -= Time.deltaTime;
            await UniTask.Yield();
        }
        _musicSource.volume = 0.0f;

        // _musicSource.Stop();
        _musicLooper.StopMusic();
        _musicSource.clip = null;

        _currentMusicId = string.Empty;
    }

    // public void PauseMusic()
    // {
    //     if (_musicSource != null)
    //     {
    //         _musicSource.Pause();
    //     }
    // }

    // public void ResumeMusic()
    // {
    //     if (_musicSource != null && _musicSource.clip != null)
    //     {
    //         _musicSource.UnPause();
    //     }
    // }
    
    
    //AMBIANCE
    
    public void PlayAmbiance(string ambianceId)
    {
        if (_audioDatabase == null || _ambianceSource == null)
        {
            return;
        }

        if (!_audioDatabase.TryGetAmbiance(ambianceId, out AudioClipDefinition clipDefinition))
        {
            Debug.LogWarning($"{LogPrefix} Ambiance introuvable : {ambianceId}");
            return;
        }

        if (clipDefinition.Clips == null)
        {
            return;
        }

        if (_currentAmbianceId == ambianceId && _ambianceSource.isPlaying)
        {
            return;
        }

        _currentAmbianceId = ambianceId;

        _ambianceSource.clip = clipDefinition.Clips[0];
        _ambianceSource.loop = clipDefinition.Loop;
        _ambianceSource.volume = clipDefinition.Volume;
        _ambianceSource.Play();
    }

    public async UniTask StopAmbianceAsync(float endingTime)
    {
        if (_ambianceSource == null || !IsAmbiancePlaying)
        {
            return;
        }
        
        float t = endingTime;
        float start = _ambianceSource.volume;
        float end = 0.0f;
        float alpha = 1.0f;
        
        while(t > 0.0f)
        {
            alpha = Mathf.Clamp01(t / endingTime);
            _ambianceSource.volume = Mathf.Clamp01(Mathf.Lerp(end, start, alpha));
            t -= Time.deltaTime;
            await UniTask.Yield();
        }
        _ambianceSource.volume = 0.0f;

        _ambianceSource.Stop();
        _ambianceSource.clip = null;

        _currentAmbianceId = string.Empty;
    }

    public void PauseAmbiance()
    {
        if (_ambianceSource != null)
        {
            _ambianceSource.Pause();
        }
    }

    public void ResumeAmbiance()
    {
        if (_ambianceSource != null && _ambianceSource.clip != null)
        {
            _ambianceSource.UnPause();
        }
    }

    //SFX
    public void PlaySfx(string sfxId)
    {
        if (_audioDatabase == null || _sfxSource == null)
        {
            return;
        }

        if (!_audioDatabase.TryGetSfx(sfxId, out AudioClipDefinition clipDefinition))
        {
            Debug.LogWarning($"{LogPrefix} SFX introuvable : {sfxId}");
            return;
        }

        if (clipDefinition.Clips == null)
        {
            return;
        }

        _sfxSource.PlayOneShot(clipDefinition.Clips[Random.Range(0, clipDefinition.Clips.Length)], clipDefinition.Volume);
    }
    
    public async UniTask PlaySfxAsync(string sfxId)
    {
        if (_audioDatabase == null || _sfxSource == null)
        {
            return;
        }

        if (!_audioDatabase.TryGetSfx(sfxId, out AudioClipDefinition clipDefinition))
        {
            Debug.LogWarning($"{LogPrefix} SFX introuvable : {sfxId}");
            return;
        }

        if (clipDefinition.Clips == null)
        {
            return;
        }

        AudioClip clip = clipDefinition.Clips[Random.Range(0, clipDefinition.Clips.Length)];
        
        _sfxSource.PlayOneShot(clip, clipDefinition.Volume);
        await UniTask.WaitForSeconds(clip.length);
    }
    
    //Voice
    public void PlayVoice(string voiceId)
    {
        if (_audioDatabase == null || _voiceSource == null)
        {
            return;
        }

        if (!_audioDatabase.TryGetVoice(voiceId, out AudioClipDefinition clipDefinition))
        {
            Debug.LogWarning($"{LogPrefix} SFX introuvable : {voiceId}");
            return;
        }

        if (clipDefinition.Clips == null)
        {
            return;
        }

        _voiceSource.PlayOneShot(clipDefinition.Clips[0], clipDefinition.Volume);
    }
    
    public async UniTask PlayVoiceAsync(string voiceId)
    {
        if (_audioDatabase == null || _voiceSource == null)
        {
            return;
        }

        if (!_audioDatabase.TryGetVoice(voiceId, out AudioClipDefinition clipDefinition))
        {
            Debug.LogWarning($"{LogPrefix} SFX introuvable : {voiceId}");
            return;
        }

        if (clipDefinition.Clips == null)
        {
            return;
        }

        _voiceSource.PlayOneShot(clipDefinition.Clips[0], clipDefinition.Volume);
        await UniTask.WaitForSeconds(clipDefinition.Clips[0].length);
    }


    

    private void ValidateReferences()
    {
        if (_audioDatabase == null)
        {
            Debug.LogWarning($"{LogPrefix} AudioDatabase non assign�e.");
        }

        if (_musicSource == null)
        {
            Debug.LogWarning($"{LogPrefix} Music AudioSource non assign�e.");
        }
        
        if (_ambianceSource == null)
        {
            Debug.LogWarning($"{LogPrefix} Ambiance AudioSource non assign�e.");
        }

        if (_sfxSource == null)
        {
            Debug.LogWarning($"{LogPrefix} SFX AudioSource non assign�e.");
        }
        
        if (_voiceSource == null)
        {
            Debug.LogWarning($"{LogPrefix} SFX VoiceSource non assign�e.");
        }
        
        if (_musicLooper == null)
        {
            Debug.LogWarning($"{LogPrefix} Music Looper non assign�e.");
        }
    }
}