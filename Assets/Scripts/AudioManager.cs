using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("AUDIO CLIP")]
    public AudioClip background;
    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }


}
