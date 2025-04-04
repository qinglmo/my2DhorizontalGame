using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // ���Ľṹ
    [System.Serializable]
    public class Sound
    {
        public string id;          // ��Ƶ��ʶ��
        public AudioClip clip;     // ��Ƶ��Դ
        public AudioType type;     // ���ͣ�����/��Ч��
        [Range(0, 1)] public float baseVolume = 1f; // ��������
    }

    public enum AudioType { Music, SFX }

    // ����ʵ��
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<Sound> soundLibrary = new List<Sound>(); // ��Ƶ��
    private Dictionary<string, Sound> _soundDict = new Dictionary<string, Sound>();
    private AudioSource _musicSource;   // ר������ͨ��
    private List<AudioSource> _sfxPool = new List<AudioSource>(); // ��Ч�����

    private float _masterVolume = 1f;
    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;

    void Awake()
    {
        // ������ʼ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ��ʼ������ͨ��
            _musicSource = GetComponent<AudioSource>();
            _musicSource.loop = true;

            // ������Ƶ�ֵ�
            foreach (var sound in soundLibrary)
            {
                _soundDict[sound.id] = sound;
            }

            // Ԥ����5����Чͨ��
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
    /// ������Ƶ���Զ�ѡ�����ͣ�
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
        // �ӳ��л�ȡ������ЧԴ
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
    /// ��̬����ȫ������
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
    /// ��ͣ��������
    /// </summary>
    public void PauseMusic() => _musicSource.Pause();

    /// <summary>
    /// ֹͣ������Ч
    /// </summary>
    public void StopAllSFX()
    {
        foreach (var source in _sfxPool)
        {
            source.Stop();
        }
    }
}
