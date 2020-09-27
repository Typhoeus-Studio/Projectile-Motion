
using UnityEngine;
using UnityEngine.UI;


public class Projectile : MonoBehaviour
{
    [Header("BALL SETTINGS")] 
     private float _reachTime = 2f;
     private float _height = 5f;
     private bool _workOnUpdate=true;
     private bool _bounciness=true;
    [Space(5)]
    [Header("Variables")] 
    [SerializeField] private Pool pool;
    [SerializeField] private Transform startPosIndicator, endPosIndicator;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Camera camera;
    [Space(5)]
    [Header("UI")] 
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider reachTimeSlider;
    [SerializeField] private Text reachTimeText;
    [SerializeField] private Text heightText;
    [SerializeField] private Toggle bouncinessToggle;
    [SerializeField] private Toggle workOnUpdateToggle;

    private void Start()
    {
        pool.CreateBalls();
        ChangeHeight();
        ChangeReachTime();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                endPosIndicator.transform.position = new Vector3(hit.point.x, endPosIndicator.position.y, hit.point.z);
                pool.ReturnBall().Shoot(hit.point, _height, _reachTime, _bounciness, _workOnUpdate);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                pool.ballStartPos = new Vector3(hit.point.x, pool.ballStartPos.y, hit.point.z);
                startPosIndicator.position = new Vector3(hit.point.x, startPosIndicator.position.y, hit.point.z);
            }
        }
    }


    #region UISettings

    public void ChangeHeight()
    {
        _height = heightSlider.value;
        heightText.text = _height.ToString();
    }

    public void ChangeReachTime()
    {
        _reachTime = reachTimeSlider.value;
        reachTimeText.text = _reachTime.ToString();
    }

    public void ChangeBounciness()
    {
        _bounciness = bouncinessToggle.isOn;
    }

    public void WorkOnUpdate()
    {
        _workOnUpdate = workOnUpdateToggle.isOn;
    }

    #endregion
}