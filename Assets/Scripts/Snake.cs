using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public float startSpeed = 10f;
    public float currentSpeed = 10f;
    
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;

    private List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;

    [SerializeField] private UIManager _uiManager;
    [SerializeField] private MMF_Player _mmFeedbacks;
    
    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            {
                input = Vector2Int.up;
            } 
            
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
            {
                input = Vector2Int.down;
            }
        }
        
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
            {
                input = Vector2Int.right;
            } 
            
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
            {
                input = Vector2Int.left;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time < nextUpdate) 
        {
            return;
        }
        
        if (input != Vector2Int.zero) 
        {
            direction = input;
        }
        
        for (int i = segments.Count - 1; i > 0; i--) 
        {
            segments[i].position = segments[i - 1].position;
        }
        
        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        
        transform.position = new Vector2(x, y);
        
        nextUpdate = Time.time + (1f / (currentSpeed * speedMultiplier));
    }

    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[^1].position;
        segments.Add(segment);
    }

    private void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = Vector3.zero;
        
        for (int i = 1; i < segments.Count; i++) 
        {
            Destroy(segments[i].gameObject);
        }
        
        segments.Clear();
        segments.Add(transform);
        
        for (int i = 0; i < initialSize - 1; i++) 
        {
            Grow();
        }
    }

    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y) {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            
            _uiManager.UpdateSegments(segments.Count);
            
            _mmFeedbacks.PlayFeedbacks();
        }
        
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            ResetState();
            _uiManager.Reset(segments.Count);
        }
        
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls) 
            {
                Traverse(other.transform);
            } 
            
            else 
            {
                ResetState();
            }
        }
    }

    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;

        if (direction.x != 0f) 
        {
            position.x = Mathf.RoundToInt(-wall.position.x + direction.x);
        } 
        
        else if (direction.y != 0f) 
        {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }

        transform.position = position;
    }
}