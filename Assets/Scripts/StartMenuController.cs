using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public Canvas startCanvas;
    public Canvas guideCanvas;
    HelperScript helper;

    private void Start()
    {
        helper = gameObject.AddComponent<HelperScript>();
    }
    public void OnStartClick()
    {
        helper.LoadNextScene();
    }
    public void OnExitClick()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
    public void OnGuideStartClick()
    {
        startCanvas.enabled = false;
        guideCanvas.enabled = true;

    }
    public void OnGuideExitClick()
    {
        guideCanvas.enabled = false;
        startCanvas.enabled = true;
        print("Exit");
    }
}
