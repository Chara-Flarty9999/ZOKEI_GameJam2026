using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Enums.EnemyMovement _enemyMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void Movement()
    {
        switch (_enemyMovement)
        {
            case Enums.EnemyMovement.Vertical:
                while (true)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        await System.Threading.Tasks.Task.Delay(10);
                        transform.position += new Vector3(0, -0.05f, 0);
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        await System.Threading.Tasks.Task.Delay(10);
                        transform.position += new Vector3(0, 0.05f, 0);
                    }
                }
            
            case Enums.EnemyMovement.Horizontal:
                while (true)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        await System.Threading.Tasks.Task.Delay(10);
                        transform.position += new Vector3(-0.05f, 0, 0);
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        await System.Threading.Tasks.Task.Delay(10);
                        transform.position += new Vector3(0.05f, 0, 0);
                    }
                }

        }
    }
}
