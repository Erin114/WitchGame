using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [SerializeField] AudioMixer mix;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    public void SetMaster(float val)
    {
        mix.SetFloat("music", val * -80f);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
