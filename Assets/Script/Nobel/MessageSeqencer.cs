using System.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MessageSequencer : MonoBehaviour
{
    CancellationTokenSource _cts;
    CancellationToken _token;
    [SerializeField]
    private MessagePrinter _printer = default;

    [SerializeField]
    private Actor _actor = default;

    [SerializeField]
    private TextMeshProUGUI _textUi = default;

    [SerializeField] private MessageSequenceData _sequence;

    [SerializeField] private MessageWait _messageWait;

    // _messages フィールドから表示する現在のメッセージのインデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    private void Start()
    {
        _cts = new CancellationTokenSource();
        
        MoveNext();
    }

    private async void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_printer.IsPrinting) { _printer.Skip(); }
            else { MoveNext(); }
        }
        if (!_printer.IsPrinting)
        {
            _token = _cts.Token;
            await _messageWait.AlphaChange(_token);
        }
    }

    /// <summary>
    /// 次のページに進む。
    /// 次のページが存在しない場合は無視する。
    /// </summary>
    //private void MoveNext()
    //{
    //    if (_messages is null or { Length: 0 }) { return; }

    //    if (_currentIndex + 1 < _messages.Length)
    //    {
    //        _currentIndex++;
    //        if (_charactorImages[_currentIndex] != null)
    //        {
    //            _actor._image.sprite = _charactorImages[_currentIndex];
    //        }
    //        _printer?.ShowMessage(_messages[_currentIndex]);
    //    }
    //}

    private void MoveNext()
    {
        _cts?.Cancel();
        if (_sequence == null || _sequence.messages.Length == 0) return;

        if (_currentIndex + 1 < _sequence.messages.Length)
        {
            _currentIndex++;
            var current = _sequence.messages[_currentIndex];
            _textUi.text = current.caption;
            _printer.Speed = current.speed;
            if (current.characterImage != null)
                _actor.Image.sprite = current.characterImage;

            _printer.ShowMessage(current.text);
        }
    }
}