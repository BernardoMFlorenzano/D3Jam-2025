using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // Refer�ncia aos Sliders da Scene
    public Slider musicSlider;
    public Slider sfxSlider;


    private void Start()
    {
        // Carrega os valores salvos anteriormente (ou usa 1 como valor padr�o se n�o existir)
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Adiciona eventos que ser�o chamados quando o valor do slider for alterado
        // O `delegate` chama os m�todos `SetMusicVolume` e `SetSFXVolume` quando os sliders s�o movidos
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    // Fun��o que ajusta o volume da m�sica usando o valor atual do slider
    public void SetMusicVolume()
    {
        // Acessa o AudioManager e define o novo volume da m�sica
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }

    // Fun��o que ajusta o volume dos efeitos sonoros usando o valor atual do slider
    public void SetSFXVolume()
    {
        // Acessa o AudioManager e define o novo volume dos efeitos sonoros
        AudioManager.instance.SetSFXVolume(sfxSlider.value);
    }
}
