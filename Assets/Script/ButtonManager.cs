using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    Image _image;
    [SerializeField]GameObject _blackFadeImage;
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
        await FadeOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public async void StartGame()
    {
        await FadeOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    async UniTask<bool> FadeOut()
    {
        GameObject canvas = GameObject.Find("Canvas");
        var instance = Instantiate(_blackFadeImage);
        instance.transform.SetParent(canvas.transform, false);
        _image = instance.GetComponent<Image>();
        for (float i = 0; i <= 1; i += Time.deltaTime)
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
