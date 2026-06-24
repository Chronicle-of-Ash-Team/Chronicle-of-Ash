using UnityEngine;

public class LK_FXEvent : MonoBehaviour
{
    private LandKnightBrain brain;

    [SerializeField] private Hit_Event Hit_Event;

    private void Awake()
    {
        brain = GetComponent<LandKnightBrain>();
    }

    private void Start()
    {
        brain.OnHitEvent += OnHitEvent;
    }

    private void OnHitEvent(LandKnightBrain.OnHit hit)
    {
        Hit_Event.Raise(hit.DamageContext);
    }
}
