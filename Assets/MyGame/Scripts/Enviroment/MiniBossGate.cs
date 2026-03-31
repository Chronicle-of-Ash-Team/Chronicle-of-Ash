using UnityEngine;

public class MiniBossGate : MonoBehaviour
{
    [SerializeField] private IDamageable miniBoss;
    [SerializeField] private GameObject blockGate;
    //[SerializeField] private GameObject openGate;

    private void Start()
    {
        if (miniBoss == null)
        {
            Debug.LogError("MiniBoss reference is not set in the inspector.");
            return;
        }
        blockGate.SetActive(true);
        //openGate.SetActive(false);
    }
}
