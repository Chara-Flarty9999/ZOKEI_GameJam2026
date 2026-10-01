using UnityEngine;
using DG.Tweening;
using System.Collections;
using Cysharp.Threading.Tasks;
using static ClossBlaster;

public class NormalBlaster : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    AudioSource audioSource;
    Animator m_anim;
    Rigidbody2D rigidbody2d;

    Enums.BlasterColor _blasterColor;
    Enums.MoveInfo _moveInfo;
    Enums.StartInfo _startInfo;
    public float _blasterSize;
    int _beamWait;
    int _beamLoop;
    Vector3 recoilMove;

    bool firstAnim = false;
    bool endBeamAnim = false;
    

    [SerializeField] GameObject _BeamPrefab;
    [SerializeField] AudioClip blasterSpawn = default;
    [SerializeField] AudioClip blasterbeam = default;

    // Start is called before the first frame update
    async void Start()
    {
        GameObject spawner = GameObject.Find("SpawnArea");
        KnifeSpawn spawndata = spawner.GetComponent<KnifeSpawn>();
        spriterenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        spriterenderer.material.color = new Color(1f, 1f, 1f, 0f);

        //この下の6つをwave等のスクリプトで設定してくだされ
        _startInfo = spawndata.F_place;
        _moveInfo = spawndata.M_place;
        _blasterSize = spawndata.BlasterSize;
        _beamWait = spawndata.BeamWait;
        _beamLoop = spawndata.BeamTime;
        _blasterColor = spawndata.BlasterColor;

        await NormalBlasterCharge(_startInfo,_moveInfo,_blasterSize,_beamWait, _beamLoop,_blasterColor);
    }

    public static Vector3 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async UniTask NormalBlasterCharge(Enums.StartInfo startInfo, Enums.MoveInfo moveInfo, float size, int waittime, int looplong, Enums.BlasterColor color)
    {
        
        var sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOMove(moveInfo.vector3, 0.8f).SetEase(Ease.OutQuart));
        sequence.Join(this.transform.DORotate(new Vector3(0,0,moveInfo.di), 0.8f).SetEase(Ease.OutQuart));
        sequence.Join(this.spriterenderer.material.DOFade(1f, 0.5f));

        this.transform.localScale = new Vector3(size, size);
        this.transform.position = startInfo.vector3;
        this.transform.eulerAngles = new Vector3(0, 0, startInfo.di);

        audioSource.PlayOneShot(blasterSpawn);

        bool firstmove = false;

        sequence.Play().OnComplete(() =>
        {
            firstmove = true;

        });
        await UniTask.WaitUntil(() => firstmove);

        await UniTask.WaitForSeconds(waittime);
        firstAnim = true;
        m_anim.SetBool("firstAnim", firstAnim);
        recoilMove = AngleToVector2(moveInfo.di);
        await UniTask.WaitForSeconds(0.25f);
        audioSource.PlayOneShot(blasterbeam);

        BlasterRecoil();
        var parent = this.transform;
        Instantiate(_BeamPrefab, Vector3.zero, Quaternion.identity, parent);
        await UniTask.WaitForSeconds((looplong + 2) * 10 * 0.01f);
        GameObject gameObject = transform.GetChild(0).gameObject;
        NormalBlasterBeam normalBlasterBeam = gameObject.GetComponent<NormalBlasterBeam>();
        normalBlasterBeam.NormalBlasterBeamExit();
        this.spriterenderer.material.DOFade(0f, 0.5f);
        await UniTask.WaitForSeconds(1f);
        return;
    }

    async UniTask BlasterRecoil()
    {
        for (int i = 0; i < 50; i++)
        {
            rigidbody2d.AddForce(-1 * recoilMove * 30); //ForceMode2D.Impulse
            await UniTask.WaitForSeconds(0.01f);
        }
        await UniTask.WaitForSeconds(1f);
        rigidbody2d.linearVelocity = Vector3.zero;
    }
    public void ToDestroy()
    {
        
    }
}
