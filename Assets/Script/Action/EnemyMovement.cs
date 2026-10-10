using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Enums.EnemyMovement _enemyMovement;
    [SerializeField] GameObject _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_player.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    async void Movement()
    {
        if (gameObject == null) return;
        try
        {
            switch (_enemyMovement)
            {
                case Enums.EnemyMovement.Vertical:
                    while (gameObject != null)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            if (gameObject == null) break;
                            await System.Threading.Tasks.Task.Delay(10);
                            transform.position += new Vector3(0, -0.05f, 0);
                        }
                        for (int i = 0; i < 100; i++)
                        {
                            if (gameObject == null) break;
                            await System.Threading.Tasks.Task.Delay(10);
                            transform.position += new Vector3(0, 0.05f, 0);
                        }
                    }
                    break;

                case Enums.EnemyMovement.Horizontal:
                    while (gameObject != null)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            if (gameObject == null) break;
                            await System.Threading.Tasks.Task.Delay(10);
                            transform.position += new Vector3(-0.05f, 0, 0);
                        }
                        for (int i = 0; i < 100; i++)
                        {
                            if (gameObject == null) break;
                            await System.Threading.Tasks.Task.Delay(10);
                            transform.position += new Vector3(0.05f, 0, 0);
                        }
                    }
                    break;

            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("‚ñ‚Ü‚ ‚Å‚µ‚å‚¤‚Ë");
        }

    }
}
