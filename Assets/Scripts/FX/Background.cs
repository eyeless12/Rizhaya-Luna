using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject _cameraHolder;
    private Camera _camera;
    private Transform _tf;
    private void Awake()
    {
        _cameraHolder = GameObject.FindWithTag("CameraHolder");
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _tf = GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        var position = _cameraHolder.GetComponent<Transform>().position;
        _tf.position = new Vector3(position.x, position.y, 0);
        _tf.localScale = new Vector3(_camera.orthographicSize / 10, 1, 1);
    }
}
