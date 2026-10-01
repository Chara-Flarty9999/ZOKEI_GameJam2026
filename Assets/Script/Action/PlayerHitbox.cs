using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] ButtonManager buttonManager;
    [SerializeField] GameObject _player;
    public GameObject Player => _player;
    [SerializeField] TextMeshProUGUI _textUi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            buttonManager.BeforeGameOverSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            InputSystem.actions.Disable();
            Debug.Log("Player hit by enemy!");
            // Handle player hit logic here
            buttonManager.SceneChange("GameOverScene");
        }
        if (collision.gameObject.CompareTag("Goal"))
        {
            InputSystem.actions.Disable();
            _textUi.text = "Clear!";
            await Awaitable.WaitForSecondsAsync(1f);
            buttonManager.SceneChange("DemoEndPart");
        }
    }
}
