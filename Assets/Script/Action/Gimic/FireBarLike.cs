using UnityEngine;

public class FireBarLike : MonoBehaviour
{
    [SerializeField] bool _isClockwise = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    async void FixedUpdate()
    {
        if (_isClockwise)
            transform.Rotate(0, 0, -2f);
        else
            transform.Rotate(0, 0, 2f);
        await Awaitable.WaitForSecondsAsync(0.01f);
    }
}
