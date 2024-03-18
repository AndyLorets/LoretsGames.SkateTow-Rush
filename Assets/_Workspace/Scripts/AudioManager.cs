using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _finishMusic;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _loseSound;
    [Space(10)]
    [SerializeField] private AudioSource _pickedSound;
    [SerializeField] private AudioSource _grappleSound;
    [SerializeField] private AudioSource _hitSound;
    [SerializeField] private AudioSource _startGameSound;
    [SerializeField] private AudioSource _boostSound;
    [SerializeField] private AudioSource _swipeSound;
    [SerializeField] private AudioSource _clickSound;
    [Space(10)]
    [SerializeField] private AudioSource[] _maleVoices;
    [Space(15)]
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _onButtonSprite;
    [SerializeField] private Sprite _offButtonSprite;

    private static AudioManager _instance;
    public static bool Mute;

    public static System.Action OnChangeState; 

    public enum SoundType
    {
        Picked, Grapple, Damage, Boost, Swipe, Click
    }
    private void Awake()
    {
        GameManager.onGameStarted += PlayGameMusic;
        GameManager.onFinish += PlayFinishMusic;
        GameManager.onLose += PlayLoseSound;

        int saveValue = PlayerPrefs.GetInt(nameof(Mute), 0);
        Mute = saveValue == 1 ? true : false;
    }
    void Start()
    {
        SetMuteValue();
        PlayMenuMusic(); 

        if(_instance == null)
            _instance = this;
    }
    public void ChangeState()
    {
        Mute = !Mute;
        SetMuteValue(); 

        int saveValue = Mute ? 1 : 0;
        PlayerPrefs.SetInt(nameof(Mute), saveValue);

        OnChangeState?.Invoke();
    }
    private void SetMuteValue()
    {
        _menuMusic.mute = Mute;
        _gameMusic.mute = Mute;
        _finishMusic.mute = Mute;
        _winSound.mute = Mute;
        _loseSound.mute = Mute;
        _pickedSound.mute = Mute;
        _grappleSound.mute = Mute;
        _hitSound.mute = Mute;
        _startGameSound.mute = Mute;
        _boostSound.mute = Mute;
        _swipeSound.mute = Mute;
        _clickSound.mute = Mute;

        for (int i = 0; i < _maleVoices.Length; i++)
            _maleVoices[i].mute = Mute;

        _buttonImage.sprite = Mute ? _offButtonSprite : _onButtonSprite; 
    }
    private void OnDestroy()
    {
        GameManager.onGameStarted -= PlayGameMusic;
        GameManager.onFinish -= PlayFinishMusic;
        GameManager.onLose -= PlayLoseSound;
    }
    private void PlayMenuMusic()
    {
        StartCoroutine(MusicChangeProcess(_menuMusic));
    }
    private void PlayGameMusic()
    {
        _startGameSound.Play(); 
        StartCoroutine(MusicChangeProcess(_gameMusic));
    }
    private void PlayFinishMusic()
    {
        _winSound.Play();
        _maleVoices[Random.Range(0, _maleVoices.Length)].Play();
        StartCoroutine(MusicChangeProcess(_finishMusic, 4f));
    }
    private void PlayLoseSound()
    {
        StartCoroutine(MusicChangeProcess(_loseSound));
    }
    public static void PlayOneShot(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Picked:
                _instance?._pickedSound.Play(); break;
            case SoundType.Grapple:
                _instance?._grappleSound.Play(); break;
            case SoundType.Damage:
                _instance?._hitSound.Play(); break;
            case SoundType.Boost:
                _instance?._boostSound.Play(); break;
            case SoundType.Swipe:
                _instance?._swipeSound.Play(); break;
            case SoundType.Click:
                _instance?._clickSound.Play(); break;
        }
    }
    public void PlayClickSound()
    {
        PlayOneShot(SoundType.Click); 
    }
    private IEnumerator MusicChangeProcess(AudioSource music, float delay = .6f)
    {
        StopAllMusic();
        yield return new WaitForSeconds(delay);
        music.Play();
    }
    private void StopAllMusic()
    {
        _menuMusic.Stop();
        _gameMusic.Stop(); 
        _finishMusic.Stop(); 
    }
}
