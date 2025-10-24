using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassaCena : MonoBehaviour
{
    [SerializeField] private AudioClip somUi;
    [SerializeField] private Animator animatorTransicoes;
    [SerializeField] private float tempoTransicao = 1f;
    public void TrocaCena(int index)
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        StartCoroutine(AnimFade(index));
    }

    public void SairJogo()
    {
        AudioManager.instance.PlaySFX(somUi, 0.5f);
        Application.Quit();
    }

    public IEnumerator AnimFade(int index)
    {
        animatorTransicoes.SetTrigger("Start");

        yield return new WaitForSeconds(tempoTransicao);

        SceneManager.LoadScene(index);
    }
}
