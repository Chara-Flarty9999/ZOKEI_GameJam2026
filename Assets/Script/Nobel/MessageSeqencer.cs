using System.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.ComponentModel;
using System;
using Cysharp.Threading.Tasks;

public class MessageSequencer : MonoBehaviour
{
    [SerializeField]ButtonManager _buttonManager = default;
    [NonSerialized] public static CancellationTokenSource _cts;
    CancellationToken _token;
    [SerializeField]
    private MessagePrinter _printer = default;

    [SerializeField]
    private Actor _actor = default;

    [SerializeField]
    private TextMeshProUGUI _textUi = default;

    [SerializeField] private MessageSequenceData _sequence;
    public MessageSequenceData Sequence 
    { 
        get { return _sequence; } 
        set 
        { 
            _sequence = value;
            var path = $"Assets/Resources/NovelData/{_sequence.name}.asset";

        }
    }

    [SerializeField] private MessageWait _messageWait;
    [Header("Choice UI")]
    [SerializeField] private Transform _choicesParent = default;
    [SerializeField] private Button _choiceButtonPrefab = default;
    [SerializeField] private float _choiceButtonSpacing = 60f;

    // _messages フィールドから表示する現在のメッセージのインデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    // 選択肢待ちフラグ
    private bool _awaitingChoice = false;
    bool _notChange = true;

    // choicesParent の基準アンカーポジションを保持
    private Vector2 _choicesParentBaseAnchoredPos = Vector2.zero;

    async void Start()
    {
        _actor.Image.color = new Color(1f, 1f, 1f, 0f);
        _cts = new CancellationTokenSource();
        // 保存しておく（RectTransform が必要）
        if (_choicesParent != null)
        {
            var rt = _choicesParent as RectTransform;
            if (rt != null) _choicesParentBaseAnchoredPos = rt.anchoredPosition;
        }
        await UniTask.DelayFrame(300);
        MoveNext();
    }

    private async void Update()
    {
        // 選択肢表示中はクリックで進めない（UI ボタンで選択させる）
        if (_awaitingChoice) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_printer.IsPrinting) { _printer.Skip(); }
            else { MoveNext(); }
        }

        if (!_printer.IsPrinting)
        {
            // もし現在のメッセージが選択肢タイプで、まだ選択肢を出していなければ出す
            if (_currentIndex >= 0 && _sequence != null && _currentIndex < _sequence.messages.Length)
            {
                var current = _sequence.messages[_currentIndex];
                if (current.messageType == Enums.MessageType.SceneChange) 
                {
                    // シーン遷移の場合は、シーン遷移処理を呼び出す
                    if (!string.IsNullOrEmpty(current.sceneName)&&_notChange)
                    {
                        _notChange = false;
                        // シーン遷移処理を呼び出す（例: SceneManager.LoadScene）
                        _buttonManager.SceneChange(current.sceneName);
                        await UniTask.DelayFrame(50); // 少し待つことでシーン遷移が始まる前に処理を終了させる
                    }
                    return;
                }
                if (current.messageType == Enums.MessageType.Choice && !_awaitingChoice)
                {
                    ShowChoices(current.choices);
                    return;
                }
            }

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
            {
                _actor.Image.sprite = current.characterImage;
                _actor.Image.color = new Color(1f, 1f, 1f, 1f); // 不透明にする
            }
            else
            {
                _actor.Image.color = new Color(1f, 1f, 1f, 0f); // 透明にする
                _actor.Image.sprite = null;
            }
                _printer.ShowMessage(current.text);
                // If message type is choice, we'll wait for printing to finish and then show choices in Update
                if (current.messageType == Enums.MessageType.Choice)
                {
                    // ensure choices will be shown after printing completes
                    _awaitingChoice = false; // will be set when ShowChoices is called
                }
        }
    }

    private void ShowChoices(ChoiceData[] choices)
    {
        if (choices == null || choices.Length == 0)
        {
            _awaitingChoice = false;
            ClearChoicesUI();
            return;
        }

        if (_choicesParent == null || _choiceButtonPrefab == null)
        {
            Debug.LogWarning("Choice UI not configured (choicesParent or choiceButtonPrefab is null)");
            _awaitingChoice = false;
            ClearChoicesUI();
            return;
        }

        // Clear existing buttons and restore parent pos
        ClearChoicesUI();

        // Shift parent downward based on number of choices so buttons don't overlap
        var parentRt = _choicesParent as RectTransform;
        if (parentRt != null)
        {
            float shift = (choices.Length - 1) * _choiceButtonSpacing;
            parentRt.anchoredPosition = _choicesParentBaseAnchoredPos - new Vector2(0f, shift);
        }

        // Create buttons
        for (int i = 0; i < choices.Length; i++)
        {
            var choice = choices[i];
            var go = Instantiate(_choiceButtonPrefab.gameObject, _choicesParent, false);
            var btn = go.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            int index = i; // capture
            btn.onClick.AddListener(() => OnChoiceSelected(choices[index]));

            // Try to set label: support TextMeshProUGUI or legacy Text
            var tmp = go.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = choice.choiceText;
            }
            else
            {
                var txt = go.GetComponentInChildren<UnityEngine.UI.Text>();
                if (txt != null) txt.text = choice.choiceText;
            }

            // Position button under parent using spacing
            var btnRt = go.GetComponent<RectTransform>();
            if (btnRt != null)
            {
                // place buttons vertically with top at parent and spacing downward
                btnRt.anchoredPosition = new Vector2(btnRt.anchoredPosition.x, -i * _choiceButtonSpacing);
            }
        }

        _awaitingChoice = true;
    }

    private void OnChoiceSelected(ChoiceData choice)
    {
        // Clear buttons and restore parent position
        ClearChoicesUI();
        _awaitingChoice = false;

        if (choice == null)
        {
            MoveNext();
            return;
        }

        if (choice.nextMessages != null)
        {
            // 別のメッセージシーケンスへ遷移
            _sequence = choice.nextMessages;
            _currentIndex = -1;
            MoveNext();
        }
        else
        {
            // nextMessages が無い場合は単に次へ
            MoveNext();
        }
    }

    private void ClearChoicesUI()
    {
        if (_choicesParent == null) return;

        for (int i = _choicesParent.childCount - 1; i >= 0; i--)
        {
            var child = _choicesParent.GetChild(i);
            Destroy(child.gameObject);
        }

        var parentRt = _choicesParent as RectTransform;
        if (parentRt != null)
        {
            parentRt.anchoredPosition = _choicesParentBaseAnchoredPos;
        }
    }
}