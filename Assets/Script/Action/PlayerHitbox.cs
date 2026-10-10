using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] ButtonManager buttonManager;
    [SerializeField] GameObject _player;
    [SerializeField]Collider2D _collider;
    public GameObject Player => _player;
    [SerializeField] Image _clearImg;
    public bool IsPlayerHit { get; private set; } = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsPlayerHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.LogWarning("“G‚É“–‚½‚Á‚½‚¼");
            _collider.enabled = false;
            buttonManager.BeforeGameOverSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            InputSystem.actions.Disable();

            IsPlayerHit = true;
            Debug.Log("Player hit by enemy!");
            // Handle player hit logic here
            buttonManager.SceneChange("GameOverScene");
        }
        if (collision.gameObject.CompareTag("Goal"))
        {
            InputSystem.actions.Disable();
            _clearImg.color = new Color(1,1,1,1);
            await Awaitable.WaitForSecondsAsync(1f);
            buttonManager.SceneChange("DemoEndPart");
        }
    }
}
