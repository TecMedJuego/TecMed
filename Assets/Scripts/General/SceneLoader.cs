using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
// Cargador de escenas junto con creación de Fade in/out para transiciones
public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasGroup canvasGroup;
    private float loadTime;
    private bool fadein;
    private bool fadeOut;


    void Update()
    {
        if (fadein)
        {
            canvasGroup.blocksRaycasts = true;
            if (canvasGroup.alpha < 1)
                canvasGroup.alpha += Time.deltaTime;
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = false;

                fadein = false;

            }
        }

        if (fadeOut)
        {
            canvasGroup.blocksRaycasts = true;

            if (canvasGroup.alpha > 0)
                canvasGroup.alpha -= Time.deltaTime;
            else
            {
                canvasGroup.blocksRaycasts = false;

                canvasGroup.alpha = 0;
                fadeOut = false;
            }
        }
    }
    public void Fade(bool In)
    {
        if (In)
        {
            fadein = true;
            StartCoroutine(LoadScene());
        }
        if (!In)
        {
            fadeOut = true;
        }
    }

    public void GoToResults()
    {
        StartCoroutine(FinalScene());
    }

    IEnumerator FinalScene()
    {
        Fade(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("FinalScene");
    }
    IEnumerator LoadScene()
    {

        yield return new WaitForSeconds(1);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
            SceneManager.LoadScene("GameScene");
        else
        {
            yield return new WaitForSeconds(1);
            Fade(false);

        }

    }

}
