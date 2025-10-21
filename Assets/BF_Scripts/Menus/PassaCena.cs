using UnityEngine;
using UnityEngine.SceneManagement;

public class PassaCena : MonoBehaviour
{
    public void TrocaCena(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void AtivaConfiguracao()
    {
        // Ativa
    }

    public void DesativaConfiguracao()
    {
        // Ativa
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
