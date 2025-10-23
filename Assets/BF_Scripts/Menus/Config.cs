using UnityEngine;

public class Config : MonoBehaviour
{
    [SerializeField] private GameObject config;
    [SerializeField] private AudioClip somUi;
    public void AtivaConfiguracao()
    {
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        config.SetActive(true);
    }

    public void DesativaConfiguracao()
    {
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        config.SetActive(false);
    }
}
