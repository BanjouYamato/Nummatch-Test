
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance {  get; private set; }
    public AudioSource _source;
    [SerializeField] AudioClip _btnSfx;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        float volume = PlayerPrefs.GetFloat(ValueConstant.volume, 1f);
        _source.volume = volume;
    }
    public void PlaySFX(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
    public void PlayBtnFX()
    {
        PlaySFX(_btnSfx);
    }
}
