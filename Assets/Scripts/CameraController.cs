
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private void Update()
    {
        
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        float fov = camera.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * 10;
        fov = Mathf.Clamp(fov, 15, 90);
        camera.fieldOfView = fov;
    }
}