using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventsInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;


    public static AudioManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        eventsInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }
    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.level1OST);
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventsInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }
    public void StopAllAudio()
    {
        foreach (EventInstance eventInstance in eventsInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
        if (ambienceEventInstance.isValid())
        {
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ambienceEventInstance.release();
        }
    }

    public void CleanUp()
    {
        foreach (EventInstance eventInstance in eventsInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        foreach(StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }
    private void OnDestroy()
    {
        CleanUp();
    }
}
