using UnityEngine;

public class ExemploTocaAudio : MonoBehaviour
{
    // Refer�ncia aos arquivos de �udio do jogo
    public AudioClip musicaClip;
    public AudioClip efeitoClip;

    void Start()
    {

    }

    void Update()
    {

    }

    // M�todo p�blico para tocar a m�sica de fundo
    public void PlayMusic()
    {
        // Chama o m�todo do AudioManager para tocar a m�sica selecionada
        AudioManager.instance.PlayMusic(musicaClip);
    }

    // M�todo p�blico para tocar um efeito sonoro (SFX)
    public void PlayEfeito()
    {
        // Chama o m�todo do AudioManager para tocar o som do efeito (uma vez)
        AudioManager.instance.PlaySFX(efeitoClip, 1f);
    }
}
