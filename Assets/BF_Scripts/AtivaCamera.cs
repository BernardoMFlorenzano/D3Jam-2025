using Unity.Cinemachine;
using UnityEngine;

public class AtivaCamera : MonoBehaviour
{
    [SerializeField] CinemachinePositionComposer cinemachineComposer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachineComposer.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
