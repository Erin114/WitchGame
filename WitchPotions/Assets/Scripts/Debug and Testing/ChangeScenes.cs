using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToPotionScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GoToQuestionScene()
    {
        //GameManager.Instance.currentCustomerIndex++;
        GameManager.Instance.servedPotion = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionSection");

    }

    public void GoToScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);

    }

}
