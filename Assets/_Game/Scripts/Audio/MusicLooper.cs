using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class MusicLooper : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Tooltip("Bus de sortie. Glisser le groupe depuis votre AudioMixer.")]
    public AudioMixerGroup output;

    [Header("Loop")]
    [Tooltip("Début de boucle en secondes (ex: 15.000)")]
    public double loopStart = 0.0;
    [Tooltip("Fin de boucle en secondes (ex: 76.935)")]
    public double loopEnd   = 0.0;
    public bool infiniteLoop = true;
    [Tooltip("Nombre de fois que la boucle se répète. Ignoré si Infini est coché. 0 = pas de loop.")]
    public int loopCount = 1;

    [Header("Debug")]
    [Tooltip("Démarre la lecture à ce timing en secondes. Mettre à 0 en production.")]
    public double startAt = 0.0;

    // -------------------------------------------------------------------------

    private AudioSource _src;
    private float[]     _clipData;
    private int         _channels;
    private int         _totalSamples;
    private int         _loopStartSample;
    private int         _loopEndSample;
    private int         _readPosition;
    private int         _loopsPlayed  = 0;
    private bool        _playing      = false;
    private bool        _loopingDone  = false;

    // -------------------------------------------------------------------------

    public void PlayMusic()
    {
        _src = GetComponent<AudioSource>();
        _src.outputAudioMixerGroup = output;
        _src.loop         = false;
        _src.playOnAwake  = false;
        _src.spatialBlend = 0f;
        _src.clip         = null;

        _channels        = clip.channels;
        _totalSamples    = clip.samples;
        _loopStartSample = (int)System.Math.Round(loopStart * clip.frequency);
        _loopEndSample   = (int)System.Math.Round(loopEnd   * clip.frequency);
        _readPosition    = (int)System.Math.Round(startAt   * clip.frequency);

        if (!infiniteLoop && loopCount == 0)
            _loopingDone = true;

        _clipData = new float[_totalSamples * _channels];
        clip.GetData(_clipData, 0);

        // Ajustement automatique du point de loop end pour compenser
        // le décalage interne de Unity — cherche le sample dans ±50
        // dont la valeur est la plus proche de loopStart
        float loopStartValue = _clipData[_loopStartSample * _channels];
        int   searchRange    = 50;
        int   bestOffset     = 0;
        float bestDelta      = float.MaxValue;

        for (int offset = -searchRange; offset <= searchRange; offset++)
        {
            int idx = _loopEndSample + offset;
            if (idx < 0 || idx >= _totalSamples) continue;
            float delta = Mathf.Abs(_clipData[idx * _channels] - loopStartValue);
            if (delta < bestDelta)
            {
                bestDelta  = delta;
                bestOffset = offset;
            }
        }

        _loopEndSample += bestOffset;

        _playing = true;
        _src.Play();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        System.Array.Clear(data, 0, data.Length);

        if (!_playing || _clipData == null) return;

        int samplesNeeded = data.Length / channels;

        for (int i = 0; i < samplesNeeded; i++)
        {
            if (!_loopingDone && _readPosition >= _loopEndSample)
            {
                if (infiniteLoop || _loopsPlayed < loopCount)
                {
                    _readPosition = _loopStartSample;
                    _loopsPlayed++;
                }
                else
                {
                    _loopingDone = true;
                }
            }

            if (_readPosition >= _totalSamples)
            {
                _playing = false;
                break;
            }

            for (int c = 0; c < channels; c++)
            {
                int clipChannel = c < _channels ? c : _channels - 1;
                data[i * channels + c] = _clipData[_readPosition * _channels + clipChannel] * volume;
            }

            _readPosition++;
        }
    }

    // -------------------------------------------------------------------------

    public void StopMusic()             { _playing = false; _src.Stop(); }
    //public void SetVolume(float v) { volume = v; }

    public void SetValues(AudioClipDefinition newValues)
    {
        this.clip = newValues.Clips[0];
        volume = newValues.Volume;
        infiniteLoop = newValues.Loop;
        loopStart = newValues.LoopStart;
        loopEnd = newValues.LoopEnd;
    }
}

// =============================================================================
#if UNITY_EDITOR
[CustomEditor(typeof(MusicLooper))]
public class MusicLooperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MusicLooper t = (MusicLooper)target;
        Undo.RecordObject(t, "MusicLooper");

        EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
        t.clip   = (AudioClip)EditorGUILayout.ObjectField("Clip", t.clip, typeof(AudioClip), false);
        t.volume = EditorGUILayout.Slider("Volume", t.volume, 0f, 1f);
        t.output = (AudioMixerGroup)EditorGUILayout.ObjectField("Output", t.output, typeof(AudioMixerGroup), false);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Loop", EditorStyles.boldLabel);
        t.loopStart = EditorGUILayout.DoubleField("Loop Start", t.loopStart);
        t.loopEnd   = EditorGUILayout.DoubleField("Loop End",   t.loopEnd);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Loop Count");
        t.infiniteLoop = EditorGUILayout.ToggleLeft("Infini", t.infiniteLoop, GUILayout.Width(55));
        GUI.enabled = !t.infiniteLoop;
        t.loopCount = EditorGUILayout.IntField(t.loopCount);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        if (!t.infiniteLoop)
            EditorGUILayout.HelpBox("0 = pas de loop  |  1+ = nombre de répétitions", MessageType.None);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        t.startAt = EditorGUILayout.DoubleField("Start At", t.startAt);
        EditorGUILayout.HelpBox("Démarre la lecture à ce timing. Mettre à 0 en production.", MessageType.Info);

        if (GUI.changed)
            EditorUtility.SetDirty(t);
    }
}
#endif