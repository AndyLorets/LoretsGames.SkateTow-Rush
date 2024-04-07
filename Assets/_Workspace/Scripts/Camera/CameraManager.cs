using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening; 

public class CameraManager : MonoBehaviour
{
    [SerializeField] private  CinemachineVirtualCamera _gameCamObj;
    [SerializeField] private  CinemachineVirtualCamera _waitCamObj;
    [SerializeField] private CinemachineVirtualCamera _flyCamObj;

    private static CinemachineVirtualCamera _gameCam;
    private static CinemachineVirtualCamera _waitCam;
    private static CinemachineVirtualCamera _flyCam;
    private static CinemachineVirtualCamera _currentCamera;

    public const string cam_game_name = "Camera_Game";
    public const string cam_wait_name = "Camera_WaitToStart";
    public const string cam_fly_name = "Camera_Fly";

    private CameraPostEffect _postEffects { get; set; }

    private void OnEnable()
    {
        GameManager.onFinish += ChangeToWaitCam;
        //TimerManager.onTimeChanged += PostEffectTweening;
        EmojiBehaviour.onEmoji += PostEffectTweening; 
    }
    private void OnDisable()
    {
        GameManager.onFinish -= ChangeToWaitCam;
        //TimerManager.onTimeChanged -= PostEffectTweening;
        EmojiBehaviour.onEmoji -= PostEffectTweening;
    }
    private void Awake()
    {
        _postEffects = new CameraPostEffect(GetComponent<PostProcessVolume>());   
    }
    private void Start()
    {
        _gameCam = _gameCamObj;
        _waitCam = _waitCamObj;
        _flyCam = _flyCamObj;

        ChangeCam(cam_wait_name); 
    }
    private void PostEffectTweening(bool active)
    {
        if (!active) return; 

        _postEffects.VignettEfffectTween();
        _postEffects.BloomEfffectTween();
        _postEffects.ColorGradingEfffectTween(); 
    }
    private void PostEffectTweening(string value, Sprite sprite)
    {
        _postEffects.VignettEfffectTween();
        _postEffects.BloomEfffectTween();
        _postEffects.ColorGradingEfffectTween();
        AudioManager.PlayOneShot(AudioManager.SoundType.Swipe); 
    }
    public static void ChangeCam(string name)
    {
        if (_currentCamera != null)
            _currentCamera.Priority = 0;
        _currentCamera = GetCamare(name);
        _currentCamera.Priority = 10; 
    }

    private void ChangeToWaitCam() => ChangeCam(cam_wait_name);
    private static CinemachineVirtualCamera GetCamare(string name)
    {
        switch (name)
        {
            case cam_game_name: 
                return _gameCam;
            case cam_wait_name: 
                return _waitCam;
            case cam_fly_name:
                return _flyCam;
        }

        return _currentCamera;
    }
}

public class CameraPostEffect
{
    private PostProcessVolume _processVolume;
    private Vignette _vignette = ScriptableObject.CreateInstance<Vignette>();
    private Bloom _bloom = ScriptableObject.CreateInstance<Bloom>(); 
    private ColorGrading _colorGrading = ScriptableObject.CreateInstance<ColorGrading>();

    private Tween _vignetteTween;
    private Tween _bloomTween;
    private Tween _colorGradingTween;

    private const float vignette_intensity_value = .45f;
    private const float bloom_intensity_value = 15f;
    private const float colorGrading_brightness_value = 25;
    private const float tween_duration = .15f;
    private const float tween_back_duration = 1.3f;

    public CameraPostEffect(PostProcessVolume processVolume)
    {
        _processVolume = processVolume;
        Init(); 
    }
    private void Init()
    {
        _processVolume.profile.TryGetSettings(out _vignette);
        _processVolume.profile.TryGetSettings(out _bloom);
        _processVolume.profile.TryGetSettings(out _colorGrading);

        _processVolume.enabled = false;
        _vignette.intensity.value = 0;
        _bloom.intensity.value = 0;
        _colorGrading.brightness.value = 0;
    }
    public void VignettEfffectTween()
    {
        _processVolume.enabled = true;
        _vignetteTween?.Kill(); 
        _vignetteTween = DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, vignette_intensity_value, tween_duration)
            .OnComplete(() => _vignetteTween = DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 0, tween_back_duration)
                                  .OnComplete(() => _processVolume.enabled = false));
    }
    public void BloomEfffectTween()
    {
        _processVolume.enabled = true;
        _bloomTween?.Kill(); 
        _bloomTween = DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, bloom_intensity_value, tween_duration)
            .OnComplete(() => _bloomTween = DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, 0, tween_back_duration)
                                  .OnComplete(() => _processVolume.enabled = false));
    }
    public void ColorGradingEfffectTween()
    {
        _processVolume.enabled = true;
        _colorGradingTween?.Kill();
        _colorGradingTween = DOTween.To(() => _colorGrading.brightness.value, x => _colorGrading.brightness.value = x, colorGrading_brightness_value, tween_duration)
            .OnComplete(() => _colorGradingTween = DOTween.To(() => _colorGrading.brightness.value, x => _colorGrading.brightness.value = x, 0, tween_back_duration)
                                  .OnComplete(() => _processVolume.enabled = false));
    }
}

