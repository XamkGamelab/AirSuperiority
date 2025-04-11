using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip itemPickUp;
    [SerializeField] private AudioClip gunPickUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemPickUp() 
    {
        audioSource.PlayOneShot(itemPickUp);
    }

    public void OnGunPickUp()
    {
        audioSource.PlayOneShot(gunPickUp);
    }
}
//All level music is from: https://opengameart.org/content/nes-shooter-music-5-tracks-3-jingles