using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public Animator transition;
    public Slider slider;
    public float transitionTime = 3f; // minimum load time for transition to work

    void Start(){
        CollisionCallback.TriggerCrossed += this.listenToTrigger;
    }

    void OnDestroy(){
        CollisionCallback.TriggerCrossed -= this.listenToTrigger;
    }

    void listenToTrigger(string trigger){
        if(trigger == "Next"){
            LoadNextLevel();
        } else if (trigger == "Reload") {
            ReloadLevel();
        }
    }

    public void LoadNextLevel(){
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadLevel(){
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadLevel(int index){
        StartCoroutine(LoadAsync(index));
    }

    IEnumerator LoadAsync(int index){
        slider.value = 0f;
        transition.SetBool("Start", true);
        yield return new WaitForSeconds(transitionTime);

        AsyncOperation loader = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

        // slider.SetActive(true);

        while(!loader.isDone){
            float progress = Mathf.Clamp01(loader.progress / 0.9f);
            Debug.Log(progress);
            slider.value = progress;

            yield return null; // wait a frame
        }

    }

}
