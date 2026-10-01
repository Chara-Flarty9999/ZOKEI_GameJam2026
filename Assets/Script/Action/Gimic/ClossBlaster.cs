using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ClossBlaster : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    AudioSource audioSource;

    Enums.MoveInfo _moveInfo;
    Enums.StartInfo _startInfo;
    public float _blasterSize;
    int _beamWait;
    int _beamLoop;
    Enums.BlasterColor _blasterColor;

    [SerializeField] GameObject _BeamPrefab;
    [SerializeField] Sprite whileBeam;
    [SerializeField] Sprite endBeam;
    [SerializeField] AudioClip blasterSpawn = default;
    [SerializeField] AudioClip blasterbeam = default;

    [SerializeField] float testX;
    [SerializeField] float testY;
    [SerializeField] int testDi;

    [SerializeField] float testX2;
    [SerializeField] float testY2;
    [SerializeField] int testDi2;



    void Start()
    {
        GameObject spawner = GameObject.Find("SpawnArea");
        KnifeSpawn spawndata = spawner.GetComponent<KnifeSpawn>();
        spriterenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        spriterenderer.color -= new Color(0f, 0f, 0f, 0f);
        spriterenderer.material.color = new Color(1f, 1f, 1f, 0f);

        //この下の6つをwave等のスクリプトで設定してくだされ
        _startInfo = spawndata.F_place;
        _moveInfo = spawndata.M_place;
        _blasterSize = spawndata.BlasterSize;
        _beamWait = spawndata.BeamWait;
        _beamLoop = spawndata.BeamTime;
        _blasterColor = spawndata.BlasterColor;


        ClossBlasterCharge(_startInfo, _moveInfo, _blasterSize, _beamWait, _beamLoop, _blasterColor);
    }

    /// <summary>
    /// 十字ブラスターを作動させるメソッド。初期位置や移動速度などは事前に変数に格納しておくように。
    /// </summary>
    /// <param name="startInfo">召喚時のX座標、Y座標、角度を設定する。角度は度数法でOK。座標はVectorでもOK。</param>
    /// <param name="moveInfo">ビーム発射後にブラスター本体がX、Yに対して、また角度を一秒にどれだけ動かすかを設定する。</param>
    /// <param name="size">ブラスターのサイズを設定する。基本は1でOK。</param>
    /// <param name="waittime">ブラスター召喚から発射までの遅延時間を設定する。</param>
    /// <param name="looplong">ビーム発射の処理を何回繰り返すか、つまり何秒間ビームを発射するか設定する。</param>
    /// <param name="beamcolor">白か青かオレンジを設定する。</param>
    public void ClossBlasterCharge(Enums.StartInfo startInfo, Enums.MoveInfo moveInfo, float size, int waittime, int looplong, Enums.BlasterColor beamcolor)
    {
        this.transform.localScale = new Vector3 (0,0);
        this.transform.position = startInfo.vector3;
        this.transform.eulerAngles = new Vector3(0,0, startInfo.di);
        audioSource.PlayOneShot(blasterSpawn);
        this.spriterenderer.material.DOFade(1f, 1.0f);
        this.transform.DOScale(size, 0.5f);

        Debug.Log("処理したぞ");

        StartCoroutine(ClossBlasterFire(moveInfo, waittime, looplong, beamcolor));
    }

    IEnumerator ClossBlasterFire(Enums.MoveInfo moveinfo, int wait, int looplong, Enums.BlasterColor beamcolor)
    {
        yield return new WaitForSeconds(wait);
        spriterenderer.sprite = whileBeam;
        audioSource.PlayOneShot(blasterbeam);
        Debug.Log("スプライトまでは変えました");
        //Instantiateを追加する
        var parent = this.transform;
        Instantiate(_BeamPrefab, Vector3.zero, Quaternion.identity, parent);
        yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < (looplong + 2) * 10; i++) 
        {
            Debug.Log("処理中");
            transform.position += moveinfo.vector3;
            transform.eulerAngles += new Vector3(0, 0, moveinfo.di);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("ループしたよ");
        spriterenderer.sprite = endBeam;

        GameObject gameObject = transform.GetChild(0).gameObject;
        ClossBlasterBeam clossBlasterBeam = gameObject.GetComponent<ClossBlasterBeam>();
        clossBlasterBeam.ClossBlasterBeamExit();
        this.spriterenderer.material.DOFade(0f, 0.5f);
        this.transform.DOScale(0, 0.5f);
        Invoke("ToDestroy", 1f);
    }

    public void ToDestroy()
    {
        Destroy(this.gameObject);
    }

}
