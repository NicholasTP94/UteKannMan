using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    //Ambience
    //Level 1 Soundtrack
    [field: Header("Level 1 OST")]
    [field: SerializeField] public EventReference level1OST { get; private set; }


    [field: Header("Player SFX")]
    //Player Footsteps
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    //Attack SFX
    [field: SerializeField] public EventReference lightAttack {  get; private set; }
    [field: SerializeField] public EventReference heavyAttack { get; private set; }
    //Jump SFX
    [field: SerializeField] public EventReference jump { get; private set; }

    //Objects SFX
    [field: SerializeField] public EventReference torchIdle { get; private set; }
    [field: SerializeField] public EventReference rune { get; private set; }
    [field: SerializeField] public EventReference ghouGetsHit { get; private set; }
    [field: SerializeField] public EventReference ghoulAttacks { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events scripts in the scene.");
        }
        instance = this;
    }
}
