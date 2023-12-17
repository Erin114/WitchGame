using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] AudioMixer mix;
    [SerializeField] Slider volume;
    [SerializeField] Slider SFX;
    float savedVolume;
    float savedSFX;
    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        savedVolume = PlayerPrefs.GetFloat("Volume");
        savedSFX = PlayerPrefs.GetFloat("SFX");
        volume.value = savedVolume;
        SFX.value = savedSFX;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf); }
    }

    public void SetSFX(float val)
    {
        mix.SetFloat("sfx", val * -80f);
        PlayerPrefs.SetFloat("SFX", val);
    }
    public void SetMaster(float val)
    {
        mix.SetFloat("music", val * -80f);
        PlayerPrefs.SetFloat("Volume", val);

    }
    public void Quit()
    {
        Application.Quit();
    }
}
