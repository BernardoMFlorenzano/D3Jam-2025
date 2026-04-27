using UnityEngine;

public class Pausa : MonoBehaviour
{
    public bool pausado;
    [SerializeField] private GameObject pausaMenu;
    [SerializeField] private AudioClip somUi;

    void Start()
    {
        pausado = false;
    }

    public void PausaJogo()
    {
        if (!pausado)
        {
            AudioManager.instance.PlaySFX(somUi, 0.5f);
            pausado = true;
            Time.timeScale = 0f;
            pausaMenu.SetActive(true);
        }
        else
        {
            AudioManager.instance.PlaySFX(somUi, 0.5f);
            pausado = false;
            Time.timeScale = 1f;
            pausaMenu.SetActive(false);
        }
    }
}
