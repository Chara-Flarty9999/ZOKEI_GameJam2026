using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Actor : MonoBehaviour
{
    [SerializeField]
    private Image _image = null;

    public Image Image
    {
        get => _image;
        set => _image = value;
    }

    public Coroutine FadeCoroutine(Color from, Color to, float duration, CancellationToken token = default)
        => StartCoroutine(FadeAsync(from, to, duration, token));

    private IEnumerator FadeAsync(Color from, Color to, float duration, CancellationToken token)
    {
        if (duration <= 0F)
        {
            _image.color = to;
            yield break;
        }

        for (var e = 0F; e < duration; e += Time.deltaTime)
        {
            if (token.IsCancellationRequested) { break; }

            var t = e / duration;
            _image.color = Color.Lerp(from, to, t);
            yield return null;
        }
        _image.color = to; // 最終的に確実に終了点に設定
    }

    public Coroutine FadeInCoroutine(float duration, CancellationToken token = default)
        => StartCoroutine(FadeInAsync(duration, token));

    private IEnumerator FadeInAsync(float duration, CancellationToken token)
    {
        var from = _image.color;
        from.a = 0F; // 透明な状態を開始点に設定
        var to = _image.color;
        to.a = 1F;　// 不透明な状態を終了点に設定

        yield return FadeAsync(from, to, duration, token);
    }

    public Coroutine FadeOutCoroutine(float duration, CancellationToken token = default)
        => StartCoroutine(FadeOutAsync(duration, token));

    private IEnumerator FadeOutAsync(float duration, CancellationToken token)
    {
        var from = _image.color;
        from.a = 1F; // 不透明な状態を開始点に設定
        var to = _image.color;
        to.a = 0F;　// 透明な状態を終了点に設定

        yield return FadeAsync(from, to, duration, token);
    }
}