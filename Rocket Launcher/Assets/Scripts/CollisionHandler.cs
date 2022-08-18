using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float nextSceneDelay = 1f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;

    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;

    AudioSource audioSource;
    

    bool isTransitioning = false;
    bool collisionDisabled = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();   
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if(Input.GetKey(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //toggle collision
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning || collisionDisabled)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Bumped into a collider");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default: StartCrashSequence();               
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
        audioSource.PlayOneShot(crashSFX);
        crashParticle.Play();
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", nextSceneDelay);
        audioSource.PlayOneShot(successSFX);
        successParticle.Play();
                      
    }
    
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        SceneManager.LoadScene(currentSceneIndex +1);
        
    }
}
