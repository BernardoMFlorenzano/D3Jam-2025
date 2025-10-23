using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Cria��o de uma inst�ncia est�tica para permitir acesso global ao AudioManager
    public static AudioManager instance;

    // Refer�ncia para o componente AudioSource que vai tocar a m�sica de fundo
    public AudioSource musicSource;

    // Refer�ncia para o componente AudioSource que vai tocar os efeitos sonoros (SFX)
    public AudioSource sfxSource;

    // Chaves para armazenar e recuperar os volumes usando PlayerPrefs (sistema de salvamento simples do Unity)
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";


    private void Awake()
    {
        // Garante que s� exista um AudioManager na cena (padr�o Singleton)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Impede que este objeto seja destru�do ao trocar de cena
        }
        else
        {
            Destroy(gameObject); // Se j� existir um AudioManager, destr�i este novo para evitar duplica��o
        }

        // Carrega os volumes salvos anteriormente (ou usa 1 como valor padr�o)
        SetMusicVolume(PlayerPrefs.GetFloat(MusicVolumeKey, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFXVolumeKey, 1f));
    }

    // M�todo p�blico para ajustar o volume da m�sica de fundo
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume; // Define o volume no AudioSource
        PlayerPrefs.SetFloat(MusicVolumeKey, volume); // Salva o volume para ser recuperado depois
    }

    // M�todo p�blico para ajustar o volume dos efeitos sonoros
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume; // Define o volume no AudioSource de SFX
        PlayerPrefs.SetFloat(SFXVolumeKey, volume); // Salva o volume para uso futuro
    }

    // M�todo para tocar uma m�sica de fundo
    public void PlayMusic(AudioClip clip)
    {
        // S� troca a m�sica se for diferente da atual
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip; // Define a nova m�sica
            musicSource.Play();      // Come�a a tocar
        }
    }

    // M�todo para tocar um efeito sonoro �nico 
    public void PlaySFX(AudioClip clip, float volumeMult)
    {
        sfxSource.PlayOneShot(clip, volumeMult); // Toca o som sem interromper outros que possam estar tocando
    }

    
}
