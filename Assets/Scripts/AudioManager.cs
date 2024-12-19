using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourcePrefab;

    public AudioSource PlayClip(AudioClip clip, bool loop = false)
    {
        var audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.pitch = !loop ? Random.Range(0.95f, 1.05f) : 0.9f;
        audioSource.Play();

        if (!loop)
        {
            var length = audioSource.clip.length;
            Destroy(audioSource.gameObject, length);
            return null;
        }
        
        audioSource.loop = true;
        return audioSource;
    }
}