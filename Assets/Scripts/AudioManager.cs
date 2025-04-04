using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // 核心结构
    [System.Serializable]
    public class Sound
    {
        public string id;          // 音频标识符
        public AudioClip clip;     // 音频资源
        public AudioType type;     // 类型（音乐/音效）
        [Range(0, 1)] public float baseVolume = 1f; // 基础音量
    }

    public enum AudioType { Music, SFX }

    // 单例实例
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<Sound> soundLibrary = new List<Sound>(); // 音频库
    private Dictionary<string, Sound> _soundDict = new Dictionary<string, Sound>();
    private AudioSource _musicSource;   // 专用音乐通道
    private List<AudioSource> _sfxPool = new List<AudioSource>(); // 音效对象池

    private float _masterVolume = 1f;
    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;

    void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 初始化音乐通道
            _musicSource = GetComponent<AudioSource>();
            _musicSource.loop = true;

            // 构建音频字典
            foreach (var sound in soundLibrary)
            {
                _soundDict[sound.id] = sound;
            }

            // 预创建5个音效通道
            for (int i = 0; i < 5; i++)
            {
                _sfxPool.Add(gameObject.AddComponent<AudioSource>());
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 播放音频（自动选择类型）
    /// </summary>
    public void Play(string id)
    {
        if (!_soundDict.TryGetValue(id, out Sound sound)) return;

        switch (sound.type)
        {
            case AudioType.Music:
                PlayMusic(sound);
                break;
            case AudioType.SFX:
                PlaySFX(sound);
                break;
        }
    }

    private void PlayMusic(Sound sound)
    {
        _musicSource.clip = sound.clip;
        _musicSource.volume = sound.baseVolume * _musicVolume * _masterVolume;
        _musicSource.Play();
    }

    private void PlaySFX(Sound sound)
    {
        // 从池中获取可用音效源
        var source = _sfxPool.Find(s => !s.isPlaying);
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            _sfxPool.Add(source);
        }

        source.clip = sound.clip;
        source.volume = sound.baseVolume * _sfxVolume * _masterVolume;
        source.Play();
    }

    /// <summary>
    /// 动态调整全局音量
    /// </summary>
    public void SetVolume(AudioType type, float volume)
    {
        switch (type)
        {
            case AudioType.Music:
                _musicVolume = Mathf.Clamp01(volume);
                _musicSource.volume = _musicVolume * _masterVolume;
                break;
            case AudioType.SFX:
                _sfxVolume = Mathf.Clamp01(volume);
                break;
        }
    }

    /// <summary>
    /// 暂停所有音乐
    /// </summary>
    public void PauseMusic() => _musicSource.Pause();

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllSFX()
    {
        foreach (var source in _sfxPool)
        {
            source.Stop();
        }
    }
}
