using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // Referência aos Sliders da Scene
    public Slider musicSlider;
    public Slider sfxSlider;


    private void Start()
    {
        // Carrega os valores salvos anteriormente (ou usa 1 como valor padrão se não existir)
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Adiciona eventos que serão chamados quando o valor do slider for alterado
        // O `delegate` chama os métodos `SetMusicVolume` e `SetSFXVolume` quando os sliders são movidos
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    // Função que ajusta o volume da música usando o valor atual do slider
    public void SetMusicVolume()
    {
        // Acessa o AudioManager e define o novo volume da música
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }

    // Função que ajusta o volume dos efeitos sonoros usando o valor atual do slider
    public void SetSFXVolume()
    {
        // Acessa o AudioManager e define o novo volume dos efeitos sonoros
        AudioManager.instance.SetSFXVolume(sfxSlider.value);
    }
}
