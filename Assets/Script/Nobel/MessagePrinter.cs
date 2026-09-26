using TMPro;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textUi = default;

    [SerializeField]
    private string _message = "";

    [SerializeField]
    private float _speed = 1.0F;
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }

    private float _elapsed = 0; // 文字を表示してからの経過時間
    private float _interval; // 文字毎の待ち時間

    private bool _isPrinting = false;
    public bool IsPrinting => _isPrinting;

    // _message フィールドから表示する現在の文字インデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    private void Start()
    {
        ShowMessage(_message);
    }

    private void Update()
    {
        if (_textUi is null || _message is null || _currentIndex + 1 >= _message.Length) { _isPrinting = false; return; }

        _elapsed += Time.deltaTime;
        if (_elapsed > _interval)
        {
            _elapsed = 0;

            // 次に表示するインデックス
            int nextIndex = _currentIndex + 1;

            // If next char starts a rich-text tag, append the whole tag at once.
            if (nextIndex < _message.Length && _message[nextIndex] == '<')
            {
                int endTag = _message.IndexOf('>', nextIndex);
                if (endTag != -1)
                {
                    // append full tag
                    _textUi.text += _message.Substring(nextIndex, endTag - nextIndex + 1);
                    _currentIndex = endTag;
                    return;
                }
                // If no closing '>' found treat '<' as normal char and fall through.
            }

            // 通常の1文字追加
            _currentIndex++;
            _textUi.text += _message[_currentIndex];
        }
    }

    /// <summary>
    /// 指定のメッセージを表示する。
    /// </summary>
    /// <param name="message">テキストとして表示するメッセージ。</param>
    public void ShowMessage(string message)
    {
        _interval = 1.0F / _speed;
        _currentIndex = -1;
        if (_textUi != null) _textUi.text = "";
        _isPrinting = true;
        _message = message;
    }

    public void Skip()
    {
        if (_textUi is null || _message is null) { return; }
        _textUi.text = _message;
        _currentIndex = _message.Length - 1;
        _isPrinting = false;
    }
}