using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;

    public AnimationCurve curve;
    public GameObject loadingScreen;
    public Slider slider;

    private void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(FadeIn());
        Time.timeScale = 1f;
    }

    public void FadeToNextScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex+1;

        StartCoroutine(FadeOut(scene));
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    public void FadeTo(int sceneID)
    {
        StartCoroutine(FadeOut(sceneID));
    }


    //FadeIn :commence écran noir, finit écran normal
    //FadeOut :commence écran normal, finit écran noir
    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(00,00,00,a);
            yield return 0;
        }

    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(00, 00, 00, a);
            yield return 0;
        }

        //Quand fondu terminé
        //SceneManager.LoadScene(scene);
        StartCoroutine(LoadAsync(scene));
    }

    IEnumerator FadeOut(int sceneID)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(00, 00, 00, a);
            yield return 0;
        }

        //Quand fondu terminé
        //SceneManager.LoadScene(sceneID);
        StartCoroutine(LoadAsync(sceneID));
    }

    IEnumerator LoadAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            slider.value = progress;

            yield return null;
        }
    }
    IEnumerator LoadAsync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }
}
