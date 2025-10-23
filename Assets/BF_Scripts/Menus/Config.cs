using UnityEngine;

public class Config : MonoBehaviour
{
    [SerializeField] private GameObject config;
    public void AtivaConfiguracao()
    {
        config.SetActive(true);
    }

    public void DesativaConfiguracao()
    {
        config.SetActive(false);
    }
}
