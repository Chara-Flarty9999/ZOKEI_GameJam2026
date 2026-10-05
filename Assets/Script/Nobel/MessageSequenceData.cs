using UnityEngine;

[CreateAssetMenu(fileName = "NewMessageSequence", menuName = "Novel/Message Sequence")]
public class MessageSequenceData : ScriptableObject
{
    public MessageData[] messages;
}

[System.Serializable] // ← ★これが重要！
public class MessageData
{
    [Tooltip("キャプション")]
    public string caption;

    // メッセージが通常のテキストか選択肢かを選べるようにする
    public Enums.MessageType messageType = Enums.MessageType.Text;

    [TextArea(2, 5)]  // ← （お好み）インスペクタで複数行入力を見やすく
    public string text;

    [Tooltip("一秒間で何文字表示させるかの設定")]
    public float speed;

    [Tooltip("キャラクター立ち絵画像")]

    public Sprite characterImage;

    [Tooltip("第二キャラクター立ち絵画像")]
    public Sprite secondCharacterImage;

    [Tooltip("背景。もし設定されている場合背景が変化してから文章が処理される。")]
    public Sprite backgroundImage;

    [Tooltip("選択肢リスト。MessageType が Choice のとき使用")]
    public ChoiceData[] choices;

    [Tooltip("シーン選択時にシーン名を入力")]
    public string sceneName; // シーン遷移用のフィールド
}

[System.Serializable]
public class ChoiceData
{
    [Tooltip("選択肢のテキスト")]
    public string choiceText;
    [Tooltip("選択肢を選んだときに表示するメッセージ")]
    public MessageSequenceData nextMessages;
}