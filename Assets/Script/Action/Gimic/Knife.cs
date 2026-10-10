using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ナイフと名前がついてはいるが実際は指定した方向にスプライトが飛んでいくもの。
///汎用性だけは普通に高い。
///別の空オブジェクトで_rote, _magnificationを設定し、あと座標をInstantiateで設定するだけ。
///サウンドはインスペクターで設定してください。
/// </summary>
public class Knife : MonoBehaviour
{
    CancellationTokenSource _cts;
    [SerializeField] SpriteRenderer _sprite;
    Vector2 _movement;
    [SerializeField] Rigidbody2D _rb2d;
    [SerializeField] AudioSource _audioSource;
    /// <summary>
    /// 召喚時の音
    /// </summary>
    [SerializeField] AudioClip spawn;
    /// <summary>
    /// 飛んでく時の音
    /// </summary>
    [SerializeField] AudioClip fly;
    /// <summary>
    /// ナイフが飛んでく角度
    /// </summary>
    int _rote;
    float _rotation;
    /// <summary>
    /// ナイフの加速度
    /// </summary>
    float _magnification;
    float _waitTime;

    public bool m_play = true;


    /// <summary>
    /// ベクトルから角度を取得する。
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }

    // Start is called before the first frame update
    async void Start()
    {
        //スポーン時の情報を取得する
        _audioSource = GetComponent<AudioSource>();
        _sprite = GetComponent<SpriteRenderer>();
        //GameObject spawner = GameObject.Find("SpawnArea");
        //KnifeSpawn knife = spawner.GetComponent<KnifeSpawn>();
        _rb2d = this.GetComponent<Rigidbody2D>();

        //_rote = knife.Rote;
        //_magnification = knife.Magnification;
        transform.rotation = Quaternion.Euler(0, 0, _rote);
        await Transparent();


        Destroy(gameObject, 3);

    }

    async UniTask Transparent()　//ここで召喚の挙動。ベクトルも取得している。
    {
        if (m_play == true)
        {
            //_audioSource.PlayOneShot(spawn);
            for (int i = 0; i < 30; i++)
            {
                _sprite.color = _sprite.color + new Color(0, 0, 0, 0.03f);
                _rb2d.rotation += 6;
                await UniTask.WaitForSeconds(0.01f);

            }
            await UniTask.WaitForSeconds(0.1f);
        }

        _rotation = transform.localEulerAngles.z;

        _movement = AngleToVector2(_rotation);



        //c = 1; //ここで飛んでいく挙動が開始されるようになっている。

        _audioSource.PlayOneShot(fly);

        for (int i = 0; i < 50; i++)
        {
            if (gameObject == null) break;
            _rb2d.AddForce(_movement * new Vector2(_magnification, _magnification)); //ForceMode2D.Impulse
            await UniTask.WaitForSeconds(0.01f);
        }
    }

    public void Initialize(int rote, float magnificate)
    {
        _rote = rote;
        _magnification = magnificate;
    }
}