using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;

public class AudioController : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance musicEventInstance;
    public static AudioController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {
//        InitializeMusic(FMODEvents.Instance.musicRND);
        InitializeMusic(FMODEvents.Instance.amdMusic);
        SetMusicParameter("mx_amb_type", 4);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayGunShot(Vector3 position, int guntype)
    {
        EventInstance gunshot = RuntimeManager.CreateInstance("event:/Player/firing");
        gunshot.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        gunshot.setParameterByName("guntype", guntype);
        gunshot.start();
        gunshot.release();
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetMusicParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicEventInstance.setParameterByName(parameterName, parameterValue);
        musicEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, string parameterValue)
    {

    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }
    
    //Shutdown emitters
    public void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemPickUp() 
    {
  //      audioSource.PlayOneShot(itemPickUp);
    }

    public void OnGunPickUp()
    {
 //       audioSource.PlayOneShot(gunPickUp);
    }

    public void Shotfired(string file)
    {
        switch (file)
        {
            case "Gun1Bullet":
 //               audioSource.PlayOneShot(gun1Shot);
                break;
            case "Gun2Bullet":
 //               audioSource.PlayOneShot(gun2Shot);
                break;
            case "Gun3Bullet":
 //               audioSource.PlayOneShot(gun3Shot);
                break;

            default:
                Debug.LogWarning("Unknown bullet type: " + file);
 //               audioSource.PlayOneShot(testBulletShot);
                return;
        }

    }
}
//All level music is from: https://opengameart.org/content/nes-shooter-music-5-tracks-3-jingles