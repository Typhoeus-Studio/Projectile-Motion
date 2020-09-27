
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    public List<Ball> balls;
    private bool _overSequel;
    private int _currentBall;
    [HideInInspector] public Vector3 ballStartPos;

    public void CreateBalls()
    {
        for (int i = 0; i < 16; i++)
        {
            balls.Add(Instantiate(ballPrefab));
        }

        ballStartPos = balls[0].transform.position;
    }

    public Ball ReturnBall()
    {
        while (balls[_currentBall].gameObject.activeInHierarchy && _currentBall < balls.Count)
        {
            _currentBall++;

            if (_currentBall >= balls.Count - 1)
            {
                _overSequel = true;
                _currentBall = 0;
            }

            if (_overSequel)
            {
                if (_currentBall == 0)
                {
                    balls[_currentBall].gameObject.SetActive(false);
                    balls[_currentBall].transform.position = ballStartPos;
                }

                balls[_currentBall + 1].gameObject.SetActive(false);
                balls[_currentBall + 1].transform.position = ballStartPos;
            }
        }

        balls[_currentBall].transform.position = ballStartPos;
        balls[_currentBall].gameObject.SetActive(true);
        return balls[_currentBall];
    }
}