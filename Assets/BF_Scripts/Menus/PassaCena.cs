using UnityEngine;
using UnityEngine.SceneManagement;

public class PassaCena : MonoBehaviour
{
    [SerializeField] private AudioClip somUi;
    public void TrocaCena(int index)
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        SceneManager.LoadScene(index);
    }

    public void SairJogo()
    {
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        Application.Quit();
    }
}
