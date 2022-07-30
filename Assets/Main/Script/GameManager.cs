using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] GameObject loadingScreenCanvas;
    [SerializeField] TextMeshProUGUI loadingProgressTxt;

    List<int> loadScenes = new List<int>();
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private void Start()
    {
        instance = this;
        SceneManager.LoadScene((int)SceneIndexes.Logo, LoadSceneMode.Additive);
       loadScenes.Add((int)SceneIndexes.Logo);
    }

    public async void LoadScene( int sceneID)
    {
        loadingScreenCanvas.SetActive(true);
        CanvasGroup canvasGroup = loadingScreenCanvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;        
        StartCoroutine(GetSceneLoadProgress(sceneID));
    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress(int sceneID)
    {
        CanvasGroup canvasGroup = loadingScreenCanvas.GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        scenesLoading.Clear();
        foreach (int sID in loadScenes)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(sID));
        }
        loadScenes.Clear();
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive));
        loadScenes.Add(sceneID);


        for ( int i = 0; i < scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone )
            {
                totalSceneProgress = 0;
                foreach( AsyncOperation op in scenesLoading)
                {
                    totalSceneProgress += op.progress;
                }
                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100;

                loadingProgressTxt.SetText($"Progress {(int)totalSceneProgress}%");
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }



        loadingScreenCanvas.SetActive(false);
    }
}
