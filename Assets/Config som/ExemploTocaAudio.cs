using UnityEngine;

public class ExemploTocaAudio : MonoBehaviour
{
    // Referência aos arquivos de áudio do jogo
    public AudioClip musicaClip;
    public AudioClip efeitoClip;

    void Start()
    {

    }

    void Update()
    {

    }

    // Método público para tocar a música de fundo
    public void PlayMusic()
    {
        // Chama o método do AudioManager para tocar a música selecionada
        AudioManager.instance.PlayMusic(musicaClip);
    }

    // Método público para tocar um efeito sonoro (SFX)
    public void PlayEfeito()
    {
        // Chama o método do AudioManager para tocar o som do efeito (uma vez)
        AudioManager.instance.PlaySFX(efeitoClip);
    }
}
