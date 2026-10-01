using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeDirect: MonoBehaviour
{

    SpriteRenderer mesh;
    private Vector2 movement;
    Rigidbody2D rigidbody2d;
    AudioSource audioSource;
    [SerializeField] AudioClip spawn;
    [SerializeField] AudioClip fly;
    Enums.FlyDirection _dir;
    public float magnification;
    public int rote;


    // Start is called before the first frame update
    void Start()
    {
        GameObject spawner = GameObject.Find("SpawnArea");
        KnifeSpawn knife = spawner.GetComponent<KnifeSpawn>();
        rote = knife.rote;
        magnification = knife.magnification;
        _dir = (Enums.FlyDirection)knife.movedirection;
        //_movement = new Vector2 ();
        transform.rotation = Quaternion.Euler(0, 0, rote);
        audioSource = GetComponent<AudioSource>();
        mesh = GetComponent<SpriteRenderer>();
        rigidbody2d = this.GetComponent<Rigidbody2D>();
        StartCoroutine("Transparent");
        mesh.material.color -= new Color32(0,0,0,255);
        rigidbody2d.rotation -= 180;
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
            movement = new Vector2(-1,0);
        }
        if (_dir == Enums.FlyDirection.Right)
        {
            movement = new Vector2(1, 0);
        }

        Invoke("Destroy", 6);
    }

    IEnumerator Transparent()
    {
        
        //_audioSource.PlayOneShot(spawn);
        for (int i = 0; i < 30; i++)
        {
            mesh.material.color = mesh.material.color + new Color32(0, 0, 0, 9);
            rigidbody2d.rotation += 6;
            yield return new WaitForSeconds(0.01f);
            
        }
        audioSource.PlayOneShot(fly);

        for (int i = 0; i < 50; i++)
        {
            rigidbody2d.AddForce(movement * new Vector2(magnification, magnification)); //ForceMode2D.Impulse
            yield return new WaitForSeconds(0.01f);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        
        
    }

    //消滅時エフェクト
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
