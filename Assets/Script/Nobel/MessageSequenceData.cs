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

    [TextArea(2, 5)]  // ← （お好み）インスペクタで複数行入力を見やすく
    public string text;

    [Tooltip("一秒間で何文字表示させるかの設定")]
    public float speed = 1.0f;

    public Sprite characterImage;
}