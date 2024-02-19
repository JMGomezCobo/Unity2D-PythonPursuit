using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    public Collider2D gridArea;
    private Snake _snake;

    #region • Unity methods (2)

    private void Awake()
    {
        _snake = FindObjectOfType<Snake>();
    }

    private void Start()
    {
        RandomizePosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }

    #endregion

    #region • Custom methods (1)

    private void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));
        
        while (_snake.Occupies(x, y))
        {
            x++;

            if (!(x > bounds.max.x)) continue;
            
            x = Mathf.RoundToInt(bounds.min.x);
            y++;

            if (y > bounds.max.y) 
            {
                y = Mathf.RoundToInt(bounds.min.y);
            }
        }

        transform.position = new Vector2(x, y);
    }

    #endregion
}
