using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Criação de uma instância estática para permitir acesso global ao AudioManager
    public static AudioManager instance;

    // Referência para o componente AudioSource que vai tocar a música de fundo
    public AudioSource musicSource;

    // Referência para o componente AudioSource que vai tocar os efeitos sonoros (SFX)
    public AudioSource sfxSource;

    // Chaves para armazenar e recuperar os volumes usando PlayerPrefs (sistema de salvamento simples do Unity)
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";


    private void Awake()
    {
        // Garante que só exista um AudioManager na cena (padrão Singleton)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Impede que este objeto seja destruído ao trocar de cena
        }
        else
        {
            Destroy(gameObject); // Se já existir um AudioManager, destrói este novo para evitar duplicação
        }

        // Carrega os volumes salvos anteriormente (ou usa 1 como valor padrão)
        SetMusicVolume(PlayerPrefs.GetFloat(MusicVolumeKey, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFXVolumeKey, 1f));
    }

    // Método público para ajustar o volume da música de fundo
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume; // Define o volume no AudioSource
        PlayerPrefs.SetFloat(MusicVolumeKey, volume); // Salva o volume para ser recuperado depois
    }

    // Método público para ajustar o volume dos efeitos sonoros
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume; // Define o volume no AudioSource de SFX
        PlayerPrefs.SetFloat(SFXVolumeKey, volume); // Salva o volume para uso futuro
    }

    // Método para tocar uma música de fundo
    public void PlayMusic(AudioClip clip)
    {
        // Só troca a música se for diferente da atual
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip; // Define a nova música
            musicSource.Play();      // Começa a tocar
        }
    }

    // Método para tocar um efeito sonoro único 
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip); // Toca o som sem interromper outros que possam estar tocando
    }
}
