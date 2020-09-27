using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public enum BallState
    {
        NOTMOVE,
        ONMOVE,
        BOUNCE,
    }

    private BallState _ballState;
    private Vector3 _hitPoint;
    private Vector3 _bounceHeight;
    private Vector3 _bouncyDest;
    private float _height;
    private float _interpolate;
    private float _reachTime;
    private bool _bounciness;
    private bool _workOnUpdate;
    private bool _onShoot;


    private void Update()
    {
        if (_workOnUpdate)
        {
            if (_ballState == BallState.ONMOVE)
            {
                if (Vector3.Distance(transform.position, _hitPoint) > 1)
                {
                    _interpolate += Time.deltaTime;
                    _interpolate = _interpolate % _reachTime;
                    transform.position = Parabol(transform.position, _hitPoint, _height, _interpolate / _reachTime);
                }
                else
                {
                    if (_bounciness)
                    {
                        _ballState = BallState.BOUNCE;
                    }
                    else
                    {
                        _ballState = BallState.NOTMOVE;
                    }

                    _interpolate = 0;
                }
            }
            else if (_ballState == BallState.BOUNCE)
            {
                if (Mathf.Abs(_bouncyDest.y - transform.position.y) > 1)
                {
                    _interpolate += Time.deltaTime;
                    _interpolate = _interpolate % _reachTime;

                    transform.position = Parabol(transform.position, _bouncyDest, _height / 4,
                        _interpolate / _reachTime);
                }
                else
                {
                    _ballState = BallState.NOTMOVE;
                    _interpolate = 0;
                }
            }
        }
    }


    public void Shoot(Vector3 hitPoint, float height, float reachTime, bool bounciness, bool workOnUpdate)
    {
        _hitPoint = hitPoint;
        _height = height;
        _reachTime = reachTime;
        _bounciness = bounciness;
        _workOnUpdate = workOnUpdate;
        if (_bounciness)
        {
            _bounceHeight = hitPoint - transform.position;
            _bouncyDest = hitPoint + new Vector3(0, _bounceHeight.y, 0); //Bounce on place.
        }

        if (!_workOnUpdate)
        {
            StartCoroutine(GoToDestination());
        }

        _ballState = BallState.ONMOVE;
    }


    #region Coroutine

    IEnumerator GoToDestination()
    {
        _interpolate = 0;
        _bounceHeight = _hitPoint - transform.position;
        while (Vector3.Distance(_hitPoint, transform.position) > 1)
        {
            _interpolate += Time.fixedDeltaTime;
            _interpolate = _interpolate % _reachTime;
            transform.position = Parabol(transform.position, _hitPoint, _height, _interpolate / _reachTime);
            yield return new WaitForEndOfFrame();
        }

        if (_bounciness)
        {
            StartCoroutine(Bounce(_hitPoint));
        }
    }

    IEnumerator Bounce(Vector3 hitPoint)
    {
        _interpolate = 0;
        // Vector3 bouncyDest = hitPoint + (temp / 3);//Go forward after bounce,
        _bouncyDest = hitPoint + new Vector3(0, _bounceHeight.y, 0); //Bounce on place.
        while (Mathf.Abs(_bouncyDest.y - transform.position.y) > 1)
        {
            _interpolate += Time.fixedDeltaTime;
            _interpolate = _interpolate % _reachTime;
            transform.position = Parabol(transform.position, _bouncyDest, _height / 4, _interpolate / _reachTime);
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion


    public Vector3 Parabol(Vector3 start, Vector3 end, float height, float t)
    {
        Vector3 mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, HeightCalculate(t) + Mathf.Lerp(start.y, end.y, t), mid.z);

        float HeightCalculate(float x)
        {
            float temp = -4 * height * x * x + 4 * height * x;
            return temp;
        }
    }
}