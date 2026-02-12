using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private CinemachineBasicMultiChannelPerlin cinemachineShake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Shake(float forca, float tempo)
    {
        cinemachineShake.AmplitudeGain = forca;
        StartCoroutine(Timer(tempo));
    }
    
    IEnumerator Timer(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        cinemachineShake.AmplitudeGain = 0f;
    }
}
