using UnityEngine;

public class Pausa : MonoBehaviour
{
    public bool pausado;
    [SerializeField] private GameObject pausaMenu;

    void Start()
    {
        pausado = false;
    }

    public void PausaJogo()
    {
        if (!pausado)
        {
            pausado = true;
            Time.timeScale = 0f;
            pausaMenu.SetActive(true);
        }
        else
        {
            pausado = false;
            Time.timeScale = 1f;
            pausaMenu.SetActive(false);
        }
    }
}
