using UnityEngine;

public sealed class MusicPlaylist : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] tracks;

    [SerializeField] private bool shuffleOnStart = false;
    [SerializeField] private int startIndex = 0;

    private int _index;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("[MusicPlaylist] AudioSource is missing");
            enabled = false;
            return;
        }

        audioSource.loop = false;

        if (tracks == null || tracks.Length == 0)
        {
            Debug.LogError("[MusicPlaylist] Tracks are not assigned");
            enabled = false;
            return;
        }

        _index = Mathf.Clamp(startIndex, 0, tracks.Length - 1);

        if (shuffleOnStart && tracks.Length > 1)
            Shuffle(tracks);
    }

    private void Start()
    {
        PlayCurrent();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
            PlayNext();
    }

    private void PlayCurrent()
    {
        audioSource.clip = tracks[_index];
        audioSource.Play();
    }

    private void PlayNext()
    {
        _index = (_index + 1) % tracks.Length;
        PlayCurrent();
    }

    private static void Shuffle(AudioClip[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}