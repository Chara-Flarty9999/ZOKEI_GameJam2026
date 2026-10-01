using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    Image _image;
    [SerializeField]GameObject _blackFadeImage;
    static string _beforeGameOverSceneName;
    public string BeforeGameOverSceneName
    {
        get { return _beforeGameOverSceneName; }
        set { _beforeGameOverSceneName = value; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ToTitle()
    {
        await FadeOut(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public async void StartGame()
    {
        await FadeOut(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("NovelPart");
    }
    public async void Restart()
    {
        await FadeOut(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(_beforeGameOverSceneName);
    }

    public async void SceneChange(string SceneName)
    {
        await FadeOut(1);
        try
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Scene change failed: {e.Message}");
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    async UniTask<bool> FadeOut(float second)
    {
        GameObject canvas = GameObject.Find("Canvas");
        var instance = Instantiate(_blackFadeImage);
        instance.transform.SetParent(canvas.transform, false);
        _image = instance.GetComponent<Image>();
        for (float i = 0; i <= second; i += Time.deltaTime)
        {
            _image.color = new Color(0, 0, 0, i);
            await UniTask.Yield();
        }
        return true;
    }

    async UniTask<bool> FadeIn()
    {
        GameObject canvas = GameObject.Find("Canvas");
        var instance = Instantiate(_blackFadeImage);
        instance.transform.SetParent(canvas.transform, false);
        _image = instance.GetComponent<Image>();
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            _image.color = new Color(0, 0, 0, i);
            await UniTask.Yield();
        }
        Destroy(instance);
        return true;
    }
}
