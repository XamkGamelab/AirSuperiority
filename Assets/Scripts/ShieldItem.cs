using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class ShieldItem : MonoBehaviour
{
    private StudioEventEmitter emitter;

    private void Start()
    {
        emitter = AudioController.Instance.InitializeEventEmitter(FMODEvents.Instance.pickUpIdle, this.gameObject);
//        Debug.Log(emitter);
        emitter.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            emitter.Stop();
            Destroy(this.gameObject);
    }
}
