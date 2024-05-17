using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    //private bool isPlaying = false;

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SoundManager";
                    instance = obj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        /*if (!isPlaying && clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            isPlaying = true;
            //StartCoroutine(CheckSoundFinished());
        }*/
    }

    //private IEnumerator CheckSoundFinished()
    //{
    //    yield return new WaitWhile(() => audioSource.isPlaying);
    //    isPlaying = false;
    //}
}
