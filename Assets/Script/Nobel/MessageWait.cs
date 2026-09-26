using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MessageWait : MonoBehaviour
{
    [SerializeField] Image _image = default;

    [SerializeField] MessagePrinter _messagePrinter;

    bool _waiting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task AlphaChange(CancellationToken token)
    {
        if (!_waiting) 
        {
            _waiting = true;
            for (float alpha = 0; alpha < 1; alpha += 0.01f)
            {
                _image.color = new Color(1, 1, 1, alpha);
                await Task.Delay(5);
                if (_messagePrinter.IsPrinting)
                {
                    _image.color = new Color(1, 1, 1, 0);
                    break;
                }
            }
            for (float alpha = 1; alpha > 0; alpha -= 0.01f)
            {
                _image.color = new Color(1, 1, 1, alpha);
                await Task.Delay(5);
                if (_messagePrinter.IsPrinting)
                {
                    _image.color = new Color(1, 1, 1, 0);
                    break;
                }
            }
            _waiting = false;
        }
    }
}
