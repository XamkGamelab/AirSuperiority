using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("PickUp SFX")]
    [field: SerializeField] public EventReference pickUp { get; private set; }
    [field: SerializeField] public EventReference pickUpIdle {get; private set;}
    
    [field: Header("Weapon Fire SFX")]
    [field: SerializeField] public EventReference fire { get; private set; }

    [field: Header("Music Event")]
    [field: SerializeField] public EventReference music { get; private set; }
    
    public static FMODEvents Instance;
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
}
