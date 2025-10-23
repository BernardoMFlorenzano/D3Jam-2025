using UnityEngine;
using UnityEngine.SceneManagement;

public class PassaCena : MonoBehaviour
{
    public void TrocaCena(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
