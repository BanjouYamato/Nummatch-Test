using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElSetting : MonoBehaviour
{
    [SerializeField] Slider _sfxSlide;

    private void Start()
    {
        _sfxSlide.value = SFXManager.instance._source.volume;
        _sfxSlide.onValueChanged.AddListener(OnVolumeChange);
    }
    void OnVolumeChange(float _volume)
    {
        SFXManager.instance._source.volume = _volume;
        PlayerPrefs.SetFloat(ValueConstant.volume, _volume);
        PlayerPrefs.Save();
    }
}
