using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CollisionHandlerScript : MonoBehaviour
{

    [SerializeField] AudioClip crashSound;

    [SerializeField] AudioClip finishSound;


    [SerializeField] ParticleSystem succesParticles;

    [SerializeField] ParticleSystem crashParticles;

    bool isCrashed = false;



    void OnCollisionEnter(Collision collision)
    {

        if (isCrashed) return;

        switch (collision.gameObject.tag)
        {

            case "Friendly":
                Debug.Log("Start on base");
                break;

            case "Finish":
                FinishSituation();
                break;

            case "Fuel":
                Debug.Log("Recargaste energia");
                break;

            default:
                DefaultSituation();
                break;
        }
    }

    void FinishSituation()
    {
        Debug.Log("Finish Pad");
        succesParticles.Play();
        DisableController();
        Vector3 pos = Camera.main != null ? Camera.main.transform.position : transform.position;
        AudioSource.PlayClipAtPoint(finishSound, pos);
        Invoke("NextLevel", 2f);
    }

    void DefaultSituation()
    {
        Debug.Log("Explotaste");
        crashParticles.Play();
        Vector3 pos = Camera.main != null ? Camera.main.transform.position : transform.position;
        AudioSource.PlayClipAtPoint(crashSound, pos);
        DisableController();
        Invoke("ReloadLevel", 2f);
    }


    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }

    void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Te acabaste el juego may");
            nextScene = 0;
        }

        Debug.Log(nextScene);
        SceneManager.LoadScene(nextScene);

    }

    public void DisableController()
    {
        isCrashed = true;
    }

    public bool getGetCrashed()
    {
        return isCrashed;
    }

}
