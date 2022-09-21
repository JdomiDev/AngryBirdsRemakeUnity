using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform BirdToFollow;
    [HideInInspector]
    public Vector3 StartingPosition;
    Camera cam;
    // Use this for initialization
    private void Start()
    {
        StartingPosition = transform.position;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsFollowing)
        {
            if (BirdToFollow != null) //bird will be destroyed if it goes out of the scene
            {
                cam.orthographicSize = GetComponent<CameraPinchToZoom>().orthographicSizeMax;
                var birdPosition = BirdToFollow.transform.position;
                float x = Mathf.Clamp(birdPosition.x, 36f / (cam.orthographicSize - (GetComponent<CameraPinchToZoom>().orthographicSizeMin - 2)) * - 1, (cam.orthographicSize - (GetComponent<CameraPinchToZoom>().orthographicSizeMin - 2)));
                //camera follows bird's x position
                transform.DOMove(new Vector3(x, StartingPosition.y, StartingPosition.z), 1f);
            }
            else
                IsFollowing = false;
        }
    }
}
