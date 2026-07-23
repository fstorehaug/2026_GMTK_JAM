using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceGun;
    public AudioSource audioSourceWalk;
    public AudioSource audioSourceDie;
    public AudioSource audioSourceWin;
    public AudioSource audioSourceMusic;

    public void playAudio(int i)
    {
        switch (i)
        {
            case 0:
                audioSourceGun.Play();
                break;
            case 1:
                audioSourceWalk.Play();
                break;
            case 2:
                audioSourceDie.Play();
                break;
            case 3:
                audioSourceWin.Play();
                break;
            case 4:
                audioSourceMusic.Play();
                break;
        }
    }

    public void stopAudio(int i)
    {
        switch (i)
        {
            case 0:
                audioSourceGun.Stop();
                break;
            case 1:
                audioSourceWalk.Stop();
                break;
            case 2:
                audioSourceDie.Stop();
                break;
            case 3:
                audioSourceWin.Stop();
                break;
            case 4:
                audioSourceMusic.Stop();
                break;
        }
    }

    public Coroutine fade(int i, float targetVolume, float duration)
    {
        return StartCoroutine(FadeRoutine(i, targetVolume, duration));
    }
    private IEnumerator FadeRoutine(int i, float targetVolume, float duration)
    {
        AudioSource audioSource;
        switch (i)
        {
            case 0:
                audioSource = audioSourceGun;
                break;
            case 1:
                audioSource = audioSourceWalk;
                break;
            case 2:
                audioSource = audioSourceDie;
                break;
            case 3:
                audioSource = audioSourceWin;
                break;
            case 4:
                audioSource = audioSourceMusic;
                break;

                float startVolume = audioSource.volume;
                float currentTime = 0f;

                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                    yield return null;
                }

                // Ensure target volume is precisely met at the end
                audioSource.volume = targetVolume;

                // Optional: Stop the source completely if fading out to 0
                if (targetVolume <= 0f)
                {
                    audioSource.Stop();
                }
        }
    }
}
