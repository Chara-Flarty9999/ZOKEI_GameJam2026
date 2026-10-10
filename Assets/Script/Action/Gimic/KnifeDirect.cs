using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeDirect: MonoBehaviour
{

    [SerializeField] SpriteRenderer _sprite;
    private Vector2 movement;
    [SerializeField] Rigidbody2D _rb2d;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip spawn;
    [SerializeField] AudioClip fly;
    Enums.FlyDirection _dir;
    public float _magnification;
    int _rote;


    // Start is called before the first frame update
    async void Start()
    {
        //GameObject spawner = GameObject.Find("SpawnArea");
        //KnifeSpawn knife = spawner.GetComponent<KnifeSpawn>();
        //_rote = knife.Rote;
        //_magnification = knife.Magnification;
        //_dir = (Enums.FlyDirection)knife.Movedirection;
        //_movement = new Vector2 ();
        transform.rotation = Quaternion.Euler(0, 0, _rote);
        _audioSource = GetComponent<AudioSource>();
        _sprite = GetComponent<SpriteRenderer>();
        _rb2d = this.GetComponent<Rigidbody2D>();
        _sprite.color = new Color32(255, 255, 255, 0);
        if (_dir == Enums.FlyDirection.Up)
        {
            movement = new Vector2(0, 1);
        }
        if (_dir == Enums.FlyDirection.Down)
        {
            movement = new Vector2(0, -1);
        }
        if (_dir == Enums.FlyDirection.Left)
        {
            movement = new Vector2(-1, 0);
        }
        if (_dir == Enums.FlyDirection.Right)
        {
            movement = new Vector2(1, 0);
        }
        await Transparent();
        Destroy(gameObject, 3);
    }

    async UniTask Transparent()
    {
        
        //_audioSource.PlayOneShot(spawn);
        for (int i = 0; i < 31; i++)
        {
            _sprite.color = _sprite.color + new Color32(0, 0, 0, 9);
            _rb2d.rotation += 6;
            await UniTask.WaitForSeconds(0.01f);
            
        }
        _audioSource.PlayOneShot(fly);

        for (int i = 0; i < 50; i++)
        {
            _rb2d.AddForce(movement * new Vector2(_magnification, _magnification)); //ForceMode2D.Impulse
            await UniTask.WaitForSeconds(0.01f);
        }
    }
    public void Initialize(int rote, int movedir, float magnificate)
    {
        _rote = rote;
        _dir = (Enums.FlyDirection)movedir;
        _magnification = magnificate;
    }
}
