using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// 書き方の備忘録
// ・ナイフ等の発射物を生成するスクリプト。
// x,yで発射座標を指定し、roteで角度を指定する。movedirectionで進行方向を指定する。
// 角度にかかわらず方向を指定できるナイフに使用するmovedirectionは0が上、1が下、2が左、3が右。
//
//
//
//
//
//
//
//

public class KnifeSpawn : MonoBehaviour
{

    [SerializeField] UnityEvent _actions;
    float y;
    float x;
    
    //ブラスター専用の情報
    /// <summary>ブラスターの初期位置</summary>
    Enums.StartInfo _f_place;
    /// <summary>ブラスターの移動情報</summary>
    Enums.MoveInfo _m_place;
    /// <summary>ブラスターのサイズ</summary>
    float _blasterSize;
    /// <summary>ブラスター発射までの遅延時間</summary>
    int _beamWait;
    /// <summary>ブラスターの発射期間(ループ回数)</summary>
    int beamTime;
    public Enums.StartInfo F_place => _f_place;

    public Enums.MoveInfo M_place => _m_place;

    public float BlasterSize => _blasterSize;

    public int BeamWait => _beamWait;

    public int BeamTime => beamTime;

    /// <summary>ブラスターの色の指定</summary>
    Enums.BlasterColor _blasterColor;
    public Enums.BlasterColor BlasterColor => _blasterColor;


    /// <summary>
    /// 直進するナイフ。
    /// </summary>
    [SerializeField] GameObject _knifePrefab = default;
    /// <summary>
    /// 進行方向が指定できるナイフ。(上下左右)
    /// </summary>
    [SerializeField] GameObject _dirKnifePrefab = default;
    /// <summary>
    /// 炸裂弾。
    /// </summary>
    [SerializeField] GameObject _bombPrefab = default;
    /// <summary>
    /// クロスブラスター
    /// </summary>
    [SerializeField] GameObject _clossBlasterPrefab = default;
    /// <summary>
    /// 通常ブラスター
    /// </summary>
    [SerializeField] GameObject _normalBlasterPrefab = default;


    bool _isPlayerFound = false;
    public int Rote { get; private set; }
    public float BlastWaitTime { get; private set; }
    public float Magnification { get; private set; }
    [SerializeField] GameObject _player;
    [SerializeField] PlayerHitbox playerHB;
    [SerializeField] PlayerMovement playerMovement;
    public int Movedirection { get; private set; } = 0;
    public int bulletNumber;

    [SerializeField] AudioClip _heal;
    AudioSource _AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();

        _actions.Invoke();
    }

    async UniTask Test()
    {
        Magnification = 20f;
        for (int i = 0; i < 30; i++)
        {

            //transform.position = Vector3.zero;
            Rote = UnityEngine.Random.Range(-150, -210);
            BlastWaitTime = 0.2f;
            float x = UnityEngine.Random.Range(10.0f, 15.0f);
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);

            await UniTask.WaitForSeconds(0.2f);

        }

        _f_place = new Enums.StartInfo(-3, -3, 0);
        _m_place = new Enums.MoveInfo(0, 0, 0);
        _blasterSize = 1.2f;
        _beamWait = 1;
        beamTime = 1;
        Instantiate(_clossBlasterPrefab);
        await UniTask.WaitForEndOfFrame();
        _f_place = new Enums.StartInfo(3, 3, 0);
        _m_place = new Enums.MoveInfo(0, 0, 0);
        _blasterSize = 1.2f;
        _beamWait = 1;
        beamTime = 1;
        Instantiate(_clossBlasterPrefab);
        await UniTask.WaitForSeconds(1f);
        int gb_di = 0;

        for (int i = 0; i < 20; i++)
        {
            Vector3 rotatevector3 = AngleToVector2(gb_di);
            _f_place = new Enums.StartInfo(rotatevector3 * -10, gb_di);
            _m_place = new Enums.MoveInfo(rotatevector3 * -10, gb_di);
            _blasterSize = 0.7f;
            _beamWait = 0;
            beamTime = 5;
            Instantiate(_normalBlasterPrefab);
            await UniTask.WaitForSeconds(0.1f);
            gb_di += 14;
        }

        Magnification = 20f;

        await UniTask.WaitForSeconds(0.2f);

        _f_place = new Enums.StartInfo(-15, 0, 180);
        _m_place = new Enums.MoveInfo(-5, 0, 0);
        _blasterSize = 1f;
        _beamWait = 3;
        beamTime = 20;
        Instantiate(_normalBlasterPrefab);
        await UniTask.WaitForSeconds(0.1f);

        //for (int i = 0; i < 30; i++)
        //{

        //    //transform.position = Vector3.zero;
        //    Rote = 180;
        //    BlastWaitTime = 0.2f;
        //    float x = UnityEngine.Random.Range(10.0f, 15.0f);
        //    y = UnityEngine.Random.Range(-6.0f, 6.0f);
        //    Vector3 spawn = new Vector3(x, y);
        //    Instantiate(_knifePrefab, spawn, Quaternion.identity);

        //    yield return new WaitForSeconds(0.2f);

        //}

    }

    async UniTask Hard()
    {
        Magnification = 20f;

        for (int i = 0; i < 30; i++)
        {
             
            //transform.position = Vector3.zero;
            Rote = UnityEngine.Random.Range(150, 210);
            BlastWaitTime = 0.2f;
            float x = UnityEngine.Random.Range(10.0f, 15.0f);
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);

            await UniTask.WaitForSeconds(0.2f);

        }



        await UniTask.WaitForSeconds(0.4f);
        y = 6;
        for (int i = 0; i < 10; i++)
        {
            Rote = 180;
            x = 12;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);


            await UniTask.WaitForSeconds(0.1f);
            y -= 1;
        }
        y = -6;
        await UniTask.WaitForSeconds(0.4f);

        for (int i = 0; i < 10; i++)
        {
            Rote = 180;
            x = 12;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);


            await UniTask.WaitForSeconds(0.1f);
            y += 1;
        }

        await UniTask.WaitForSeconds(0.6f);

        for (int i = 0; i < 10; i++)
        {
            x = 12;
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Rote = 200;
            for (int j = 0; j < 10; j++)
            {
                Vector3 spawn = new Vector3(x, y);
                Instantiate(_knifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                Rote -= 5;
            }
            await UniTask.WaitForSeconds(0.7f);
        }
        x = 12;
        for (int i = 0; i < 5; i++)
        {
            Magnification = 8;
            for (int j = 0; j < 2; j++)
            {
                y = 0;
                Rote = 90;
                Movedirection = 2;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(_dirKnifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.4f);
                y = 6;
                Rote = 90;
                Movedirection = 2;
                spawn = new Vector3(x, y);
                Instantiate(_dirKnifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y = -6;
                Rote = 90;
                Movedirection = 2;
                spawn = new Vector3(x, y);
                Instantiate(_dirKnifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.4f);
            }
            y = 4;
            Magnification = 20;
            for (int m = 0; m < 6; m++)
            {
                Rote = 180;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(_knifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y -= 0.5f;
            }
            y = -4.5f;
            Magnification = 20;
            for (int m = 0; m < 6; m++)
            {
                Rote = 180;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(_knifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y += 0.5f;
            }
        }
        await UniTask.WaitForSeconds(0.6f);
        Rote = 180;
        for (int m = 0; m < 50; m++)
        {
            x = -12;
            y = 5;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            y = -5;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            x = 12;
            y = 5;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            y = -5;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.1f);
            Rote -= 35;
        }
        await UniTask.WaitForSeconds(0.5f);

        for (int m = 0; m <= 25; m++)
        {
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            x = UnityEngine.Random.Range(-13.0f, 13.0f);
            Vector2 spawn = new Vector2(x, y);
            //Vector2 dir = _player.transform.position - spawn;
            transform.position = spawn;
            this.transform.LookAt(_player.transform);
            Quaternion dir = this.transform.rotation;
            Rote = (int)(Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.2f);
        }
        for (int m = 0; m <= 40; m++)
        {
            x = 13;
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector2 spawn = new Vector2(x, y);
            Rote = 180;
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            x = -13;
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            spawn = new Vector2(x, y);
            Rote = 0;
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.3f);
        }

        _AudioSource.PlayOneShot(_heal);
        await UniTask.WaitForSeconds(0.9f);

        for (int i = 0; i < 20; i++)
        {

            //transform.position = Vector3.zero;
            Rote = UnityEngine.Random.Range(0, 360);
            BlastWaitTime = 0.2f;
            bulletNumber = 8;
            float x = UnityEngine.Random.Range(-12.0f, 12.0f);
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);

            await UniTask.WaitForSeconds(0.4f);

        }

        await UniTask.WaitForSeconds(0.8f);

        for (int i = 0; i < 4; i++)
        {
            float y = _player.transform.position.y;
            //transform.position = Vector3.zero;
            Rote = UnityEngine.Random.Range(0, 360);
            BlastWaitTime = 0.8f;
            bulletNumber = 12;
            float x = _player.transform.position.x;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(1f);


            y = _player.transform.position.y;
            x = _player.transform.position.x - 5;
            Rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y;
            x = _player.transform.position.x + 5;
            Rote = 180;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y - 5;
            x = _player.transform.position.x;
            Rote = 90;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y + 5;
            x = _player.transform.position.x;
            Rote = 270;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            await UniTask.WaitForSeconds(0.8f);

            y = _player.transform.position.y;
            //transform.position = Vector3.zero;
            Rote = UnityEngine.Random.Range(0, 360);
            bulletNumber = 12;
            x = _player.transform.position.x;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(1f);


            y = _player.transform.position.y + 5;
            x = _player.transform.position.x - 5;
            Rote = 315;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y + 5;
            x = _player.transform.position.x + 5;
            Rote = 225;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y - 5;
            x = _player.transform.position.x - 5;
            Rote = 45;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y - 5;
            x = _player.transform.position.x + 5;
            Rote = 135;
            spawn = new Vector3(x, y);
            Instantiate(_knifePrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            await UniTask.WaitForSeconds(0.8f);

        }

        BlastWaitTime = 0.2f;

        for (int i = 0; i < 3; i++)
        {
            y = 0;
            x = 8;
            Rote = 0;
            bulletNumber = 8;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = 3;
            x = 11;
            Rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -3;
            x = 11;
            Rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.6f);

            for (int j = 0; j < 8; j++)
            {
                y = UnityEngine.Random.Range(-5, 5);
                x = 12;
                Magnification = 17;
                Rote = 90;
                Movedirection = 2;
                spawn = new Vector3(x, y);
                Instantiate(_dirKnifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.2f);
            }

            await UniTask.WaitForSeconds(0.6f);

            y = 0;
            x = -8;
            Rote = 0;
            bulletNumber = 8;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = 3;
            x = -11;
            Rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -3;
            x = -11;
            Rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.6f);

            for (int j = 0; j < 8; j++)
            {
                y = UnityEngine.Random.Range(-5, 5);
                x = -12;
                Magnification = 17;
                Rote = 90;
                Movedirection = 3;
                spawn = new Vector3(x, y);
                Instantiate(_dirKnifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.2f);
            }

            await UniTask.WaitForSeconds(0.6f);
        }


        for (int i = 0; i < 5; i++)
        {
            y = 5;
            x = 7;
            Rote = UnityEngine.Random.Range(0, 360);
            bulletNumber = 10;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -5;
            x = 7;
            Rote = UnityEngine.Random.Range(0, 360);
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = 5;
            x = -7;
            Rote = UnityEngine.Random.Range(0, 360);
            bulletNumber = 10;
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -5;
            x = -7;
            Rote = UnityEngine.Random.Range(0, 360);
            spawn = new Vector3(x, y);
            Instantiate(_bombPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            for (int j = 0; j < 6; j++)
            {
                float x = 11f;
                float y = _player.transform.position.y;
                Magnification = 25;
                Rote = 180;
                spawn = new Vector3(x, y);
                Instantiate(_knifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                for (int k = 0; k < 6; k++)
                {
                    y = 2.7f;
                    x = 11;
                    Magnification = 25;
                    Rote = 180;
                    spawn = new Vector3(x, y);
                    Instantiate(_knifePrefab, spawn, Quaternion.identity);
                    await UniTask.WaitForEndOfFrame();
                    y = -2.7f;
                    spawn = new Vector3(x, y);
                    Instantiate(_knifePrefab, spawn, Quaternion.identity);
                    await UniTask.WaitForSeconds(0.1f);
                }

            }
        }


        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                y = _player.transform.position.y;
                x = 11;
                Magnification = 25;
                Rote = 180;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(_knifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y = 7;
                x = _player.transform.position.x;
                Rote = 270;
                spawn = new Vector3(x, y);
                Instantiate(_knifePrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.05f);

            }
            for (x = -11; x <= 11; x += 4)
            {
                bulletNumber = 10;
                y = 6;
                Rote = UnityEngine.Random.Range(0, 360);
                Vector3 spawn = new Vector3(x, y);
                Instantiate(_bombPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y = -6;
                Rote = UnityEngine.Random.Range(0, 360);
                spawn = new Vector3(x, y);
                Instantiate(_bombPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.4f);
            }

            await UniTask.WaitForSeconds(0.8f);
        }



    }
    // Update is called once per frame
    public void BlasterTest()
    {
        StartCoroutine("Test");
    }

    public static Vector3 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }

    public async void TracePencil()
    {
        try
        {
            await UniTask.WaitUntil(() => _isPlayerFound);
            while (_isPlayerFound)
            {
                Magnification = 10f;
                for (int m = 0; m <= 2; m++)
                {
                    if (playerHB.IsPlayerHit) break;
                    y = gameObject.transform.position.y;
                    x = gameObject.transform.position.x;
                    Vector2 spawn = new Vector2(x, y);
                    transform.position = spawn;
                    Vector2 dir = (Vector2)_player.transform.position - spawn;
                    Rote = (int)(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 180;
                    var knifeObj = Instantiate(_knifePrefab, spawn, Quaternion.identity).GetComponent<Knife>();
                    knifeObj.Initialize(Rote, Magnification);
                    await UniTask.WaitForSeconds(1f);
                }
                if (playerHB.IsPlayerHit)
                {
                    break;
                }
            }
            if (!playerHB.IsPlayerHit)
            {
                TracePencil();
            }
            
        }
        catch (Exception e)
        {

        }
    }

    public async void HeartAttack()
    {
        try
        {
            await UniTask.WaitUntil(() => _isPlayerFound);
            while (_isPlayerFound)
            {
                Magnification = 8f;
                for (int m = 0; m <= 2; m++)
                {
                    if (playerHB.IsPlayerHit) break;
                    int randompos = UnityEngine.Random.Range(0, 3);
                    if (randompos < 1) 
                    {
                        y = -3;
                    }
                    else if (randompos == 1)
                    {
                        y = -0.6f;
                    }
                    else
                    {
                        y = 1.8f;
                    }
                    x = gameObject.transform.position.x;
                    Vector2 spawn = new Vector2(x, y);
                    Rote = 90;
                    Movedirection = 2;
                    var knifeObj = Instantiate(_dirKnifePrefab, spawn, Quaternion.identity).GetComponent<KnifeDirect>();
                    knifeObj.Initialize(Rote, Movedirection, Magnification);
                    await UniTask.WaitForSeconds(2f);
                }
                if (playerHB.IsPlayerHit)
                {
                    break;
                }
            }
            if (!playerHB.IsPlayerHit)
            {
                HeartAttack();
            }
        }
        catch (Exception e)
        {

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isPlayerFound = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isPlayerFound = false;
        }
    }
}
