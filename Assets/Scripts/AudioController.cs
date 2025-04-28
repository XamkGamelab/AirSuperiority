using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip itemPickUp;
    [SerializeField] private AudioClip gunPickUp;
    [SerializeField] private AudioClip gun1Shot;
    [SerializeField] private AudioClip gun2Shot;
    [SerializeField] private AudioClip gun3Shot;
    [SerializeField] private AudioClip testBulletShot;

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

    public void Shotfired(string file)
    {
        switch (file)
        {
            case "Gun1Bullet":
                audioSource.PlayOneShot(gun1Shot);
                break;
            case "Gun2Bullet":
                audioSource.PlayOneShot(gun2Shot);
                break;
            case "Gun3Bullet":
                audioSource.PlayOneShot(gun3Shot);
                break;

            default:
                Debug.LogWarning("Unknown bullet type: " + file);
                audioSource.PlayOneShot(testBulletShot);
                return;
        }

    }
}
//All level music is from: https://opengameart.org/content/nes-shooter-music-5-tracks-3-jingles