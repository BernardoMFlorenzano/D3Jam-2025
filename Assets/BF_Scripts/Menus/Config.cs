using UnityEngine;

public class Config : MonoBehaviour
{
    [SerializeField] private GameObject config;
    [SerializeField] private GameObject botoes;
    [SerializeField] private AudioClip somUi;
    public void AtivaConfiguracao()
    {
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        config.SetActive(true);
        if (botoes)
            botoes.SetActive(false);
    }

    public void DesativaConfiguracao()
    {
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        config.SetActive(false);
        if (botoes)
            botoes.SetActive(true);
    }
}
