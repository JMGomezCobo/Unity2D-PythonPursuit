using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private int score;

    private int _currentScore;
    private int _currentSegments;
    
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _currentSegmentsText;

    private void Start()
    {
        Reset(4);
    }

    public void Reset(int segmentsCount)
    {
        _currentScore = 0;
        _currentScoreText.text = _currentScore.ToString();
        
        _currentSegmentsText.text = segmentsCount.ToString();
        
    }
    
    public void UpdateSegments(int segmentsCount)
    {
        _currentScore += score;
        _currentScoreText.text = _currentScore.ToString();
        
        _currentSegmentsText.text = segmentsCount.ToString();
    }
}
