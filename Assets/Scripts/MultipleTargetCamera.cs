using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> players;
    public Vector3 offset;
    
    private Vector3 _velocity;
    private Camera _cam;
    public bool zoomEnabled; //FIX

    [SerializeField] private float smoothTime = .5f;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomCoefficient;

    private void Start()
    {
        _cam = GetComponent<Camera>();
        players = new List<Transform>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (arg0, mode) => zoomEnabled = true;
    }

    private void LateUpdate()
    {
        if (players.Count == 0 || !zoomEnabled)
            return;
        
        Move();
        Zoom();
    }

    private void Move()
    {
        var centerPoint = GetCenterPoint();
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            centerPoint + offset,
            ref _velocity, 
            smoothTime);
    }

    private void Zoom()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);
        foreach (var t in players)
            bounds.Encapsulate(t.position);
        var distance = bounds.size.x;
        

        var newZoom = Mathf.Lerp(maxZoom, minZoom, distance / 50f) / zoomCoefficient;
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, newZoom, Time.deltaTime);
    }

    private Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
            return players[0].position;

        var bounds = new Bounds(players[0].position, Vector3.zero);
        foreach (var t in players)
            bounds.Encapsulate(t.position);
        
        return bounds.center; 
    }
}
