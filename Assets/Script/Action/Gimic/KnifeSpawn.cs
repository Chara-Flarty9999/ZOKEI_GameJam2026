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
    public int damage; //特に使ってない

    float y;
    float x;
    
    //ブラスター専用の情報
    /// <summary>ブラスターの初期位置</summary>
    Enums.StartInfo f_place;
    /// <summary>ブラスターの移動情報</summary>
    Enums.MoveInfo m_place;
    /// <summary>ブラスターのサイズ</summary>
    float blasterSize;
    /// <summary>ブラスター発射までの遅延時間</summary>
    int _beamWait;
    /// <summary>ブラスターの発射期間(ループ回数)</summary>
    int beamTime;
    public Enums.StartInfo F_place => f_place;

    public Enums.MoveInfo M_place => m_place;

    public float BlasterSize => blasterSize;

    public int BeamWait => _beamWait;

    public int BeamTime => beamTime;

    /// <summary>ブラスターの色の指定</summary>
    Enums.BlasterColor blasterColor;
    public Enums.BlasterColor BlasterColor => blasterColor;


    /// <summary>
    /// 直進するナイフ。
    /// </summary>
    [SerializeField] GameObject m_spawnPrefab = default;
    /// <summary>
    /// 進行方向が指定できるナイフ。(上下左右)
    /// </summary>
    [SerializeField] GameObject m_spawn2Prefab = default;
    /// <summary>
    /// 炸裂弾。
    /// </summary>
    [SerializeField] GameObject m_spawn3Prefab = default;
    /// <summary>
    /// クロスブラスター
    /// </summary>
    [SerializeField] GameObject m_clossBlasterPrefab = default;
    /// <summary>
    /// 通常ブラスター
    /// </summary>
    [SerializeField] GameObject m_normalBlasterPrefab = default;


    public int rote;
    public float blastWaitTime;
    public float magnification;
    GameObject _player;
    public PlayerHitbox playerHB;
    public PlayerMovement playerMovement;
    public int movedirection = 0;
    public int bulletNumber;

    [SerializeField] AudioClip _heal;
    AudioSource _AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
        playerHB = _player.GetComponent<PlayerHitbox>();
        playerMovement = _player.GetComponent<PlayerMovement>();
    }

    async UniTask Test()
    {
        magnification = 20f;
        for (int i = 0; i < 30; i++)
        {

            //transform.position = Vector3.zero;
            rote = UnityEngine.Random.Range(-150, -210);
            blastWaitTime = 0.2f;
            float x = UnityEngine.Random.Range(10.0f, 15.0f);
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);

            await UniTask.WaitForSeconds(0.2f);

        }

        f_place = new Enums.StartInfo(-3, -3, 0);
        m_place = new Enums.MoveInfo(0, 0, 0);
        blasterSize = 1.2f;
        _beamWait = 1;
        beamTime = 1;
        Instantiate(m_clossBlasterPrefab);
        await UniTask.WaitForEndOfFrame();
        f_place = new Enums.StartInfo(3, 3, 0);
        m_place = new Enums.MoveInfo(0, 0, 0);
        blasterSize = 1.2f;
        _beamWait = 1;
        beamTime = 1;
        Instantiate(m_clossBlasterPrefab);
        await UniTask.WaitForSeconds(1f);
        int gb_di = 0;

        for (int i = 0; i < 20; i++)
        {
            Vector3 rotatevector3 = AngleToVector2(gb_di);
            f_place = new Enums.StartInfo(rotatevector3 * -10, gb_di);
            m_place = new Enums.MoveInfo(rotatevector3 * -10, gb_di);
            blasterSize = 0.7f;
            _beamWait = 0;
            beamTime = 5;
            Instantiate(m_normalBlasterPrefab);
            await UniTask.WaitForSeconds(0.1f);
            gb_di += 14;
        }

        magnification = 20f;

        await UniTask.WaitForSeconds(0.2f);

        f_place = new Enums.StartInfo(-15, 0, 180);
        m_place = new Enums.MoveInfo(-5, 0, 0);
        blasterSize = 1f;
        _beamWait = 3;
        beamTime = 20;
        Instantiate(m_normalBlasterPrefab);
        await UniTask.WaitForSeconds(0.1f);

        //for (int i = 0; i < 30; i++)
        //{

        //    //transform.position = Vector3.zero;
        //    rote = 180;
        //    blastWaitTime = 0.2f;
        //    float x = UnityEngine.Random.Range(10.0f, 15.0f);
        //    y = UnityEngine.Random.Range(-6.0f, 6.0f);
        //    Vector3 spawn = new Vector3(x, y);
        //    Instantiate(m_spawnPrefab, spawn, Quaternion.identity);

        //    yield return new WaitForSeconds(0.2f);

        //}

    }

    async UniTask Hard()
    {
        magnification = 20f;

        for (int i = 0; i < 30; i++)
        {
             
            //transform.position = Vector3.zero;
            rote = UnityEngine.Random.Range(150, 210);
            blastWaitTime = 0.2f;
            float x = UnityEngine.Random.Range(10.0f, 15.0f);
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);

            await UniTask.WaitForSeconds(0.2f);

        }



        await UniTask.WaitForSeconds(0.4f);
        y = 6;
        for (int i = 0; i < 10; i++)
        {
            rote = 180;
            x = 12;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);


            await UniTask.WaitForSeconds(0.1f);
            y -= 1;
        }
        y = -6;
        await UniTask.WaitForSeconds(0.4f);

        for (int i = 0; i < 10; i++)
        {
            rote = 180;
            x = 12;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);


            await UniTask.WaitForSeconds(0.1f);
            y += 1;
        }

        await UniTask.WaitForSeconds(0.6f);

        for (int i = 0; i < 10; i++)
        {
            x = 12;
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            rote = 200;
            for (int j = 0; j < 10; j++)
            {
                Vector3 spawn = new Vector3(x, y);
                Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                rote -= 5;
            }
            await UniTask.WaitForSeconds(0.7f);
        }
        x = 12;
        for (int i = 0; i < 5; i++)
        {
            magnification = 8;
            for (int j = 0; j < 2; j++)
            {
                y = 0;
                rote = 90;
                movedirection = 2;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(m_spawn2Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.4f);
                y = 6;
                rote = 90;
                movedirection = 2;
                spawn = new Vector3(x, y);
                Instantiate(m_spawn2Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y = -6;
                rote = 90;
                movedirection = 2;
                spawn = new Vector3(x, y);
                Instantiate(m_spawn2Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.4f);
            }
            y = 4;
            magnification = 20;
            for (int m = 0; m < 6; m++)
            {
                rote = 180;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y -= 0.5f;
            }
            y = -4.5f;
            magnification = 20;
            for (int m = 0; m < 6; m++)
            {
                rote = 180;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y += 0.5f;
            }
        }
        await UniTask.WaitForSeconds(0.6f);
        rote = 180;
        for (int m = 0; m < 50; m++)
        {
            x = -12;
            y = 5;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            y = -5;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            x = 12;
            y = 5;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            y = -5;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.1f);
            rote -= 35;
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
            rote = (int)(Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.2f);
        }
        for (int m = 0; m <= 40; m++)
        {
            x = 13;
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector2 spawn = new Vector2(x, y);
            rote = 180;
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();
            x = -13;
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            spawn = new Vector2(x, y);
            rote = 0;
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.3f);
        }

        _AudioSource.PlayOneShot(_heal);
        await UniTask.WaitForSeconds(0.9f);

        for (int i = 0; i < 20; i++)
        {

            //transform.position = Vector3.zero;
            rote = UnityEngine.Random.Range(0, 360);
            blastWaitTime = 0.2f;
            bulletNumber = 8;
            float x = UnityEngine.Random.Range(-12.0f, 12.0f);
            y = UnityEngine.Random.Range(-6.0f, 6.0f);
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);

            await UniTask.WaitForSeconds(0.4f);

        }

        await UniTask.WaitForSeconds(0.8f);

        for (int i = 0; i < 4; i++)
        {
            float y = _player.transform.position.y;
            //transform.position = Vector3.zero;
            rote = UnityEngine.Random.Range(0, 360);
            blastWaitTime = 0.8f;
            bulletNumber = 12;
            float x = _player.transform.position.x;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(1f);


            y = _player.transform.position.y;
            x = _player.transform.position.x - 5;
            rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y;
            x = _player.transform.position.x + 5;
            rote = 180;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y - 5;
            x = _player.transform.position.x;
            rote = 90;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y + 5;
            x = _player.transform.position.x;
            rote = 270;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            await UniTask.WaitForSeconds(0.8f);

            y = _player.transform.position.y;
            //transform.position = Vector3.zero;
            rote = UnityEngine.Random.Range(0, 360);
            bulletNumber = 12;
            x = _player.transform.position.x;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(1f);


            y = _player.transform.position.y + 5;
            x = _player.transform.position.x - 5;
            rote = 315;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y + 5;
            x = _player.transform.position.x + 5;
            rote = 225;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y - 5;
            x = _player.transform.position.x - 5;
            rote = 45;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = _player.transform.position.y - 5;
            x = _player.transform.position.x + 5;
            rote = 135;
            spawn = new Vector3(x, y);
            Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            await UniTask.WaitForSeconds(0.8f);

        }

        blastWaitTime = 0.2f;

        for (int i = 0; i < 3; i++)
        {
            y = 0;
            x = 8;
            rote = 0;
            bulletNumber = 8;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = 3;
            x = 11;
            rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -3;
            x = 11;
            rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.6f);

            for (int j = 0; j < 8; j++)
            {
                y = UnityEngine.Random.Range(-5, 5);
                x = 12;
                magnification = 17;
                rote = 90;
                movedirection = 2;
                spawn = new Vector3(x, y);
                Instantiate(m_spawn2Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.2f);
            }

            await UniTask.WaitForSeconds(0.6f);

            y = 0;
            x = -8;
            rote = 0;
            bulletNumber = 8;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = 3;
            x = -11;
            rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -3;
            x = -11;
            rote = 0;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForSeconds(0.6f);

            for (int j = 0; j < 8; j++)
            {
                y = UnityEngine.Random.Range(-5, 5);
                x = -12;
                magnification = 17;
                rote = 90;
                movedirection = 3;
                spawn = new Vector3(x, y);
                Instantiate(m_spawn2Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.2f);
            }

            await UniTask.WaitForSeconds(0.6f);
        }


        for (int i = 0; i < 5; i++)
        {
            y = 5;
            x = 7;
            rote = UnityEngine.Random.Range(0, 360);
            bulletNumber = 10;
            Vector3 spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -5;
            x = 7;
            rote = UnityEngine.Random.Range(0, 360);
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = 5;
            x = -7;
            rote = UnityEngine.Random.Range(0, 360);
            bulletNumber = 10;
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            y = -5;
            x = -7;
            rote = UnityEngine.Random.Range(0, 360);
            spawn = new Vector3(x, y);
            Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
            await UniTask.WaitForEndOfFrame();

            for (int j = 0; j < 6; j++)
            {
                float x = 11f;
                float y = _player.transform.position.y;
                magnification = 25;
                rote = 180;
                spawn = new Vector3(x, y);
                Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                for (int k = 0; k < 6; k++)
                {
                    y = 2.7f;
                    x = 11;
                    magnification = 25;
                    rote = 180;
                    spawn = new Vector3(x, y);
                    Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                    await UniTask.WaitForEndOfFrame();
                    y = -2.7f;
                    spawn = new Vector3(x, y);
                    Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
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
                magnification = 25;
                rote = 180;
                Vector3 spawn = new Vector3(x, y);
                Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y = 7;
                x = _player.transform.position.x;
                rote = 270;
                spawn = new Vector3(x, y);
                Instantiate(m_spawnPrefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.05f);

            }
            for (x = -11; x <= 11; x += 4)
            {
                bulletNumber = 10;
                y = 6;
                rote = UnityEngine.Random.Range(0, 360);
                Vector3 spawn = new Vector3(x, y);
                Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForEndOfFrame();
                y = -6;
                rote = UnityEngine.Random.Range(0, 360);
                spawn = new Vector3(x, y);
                Instantiate(m_spawn3Prefab, spawn, Quaternion.identity);
                await UniTask.WaitForSeconds(0.4f);
            }

            await UniTask.WaitForSeconds(0.8f);
        }



    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlasterTest()
    {
        StartCoroutine("Test");
    }

    public static Vector3 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }
}
