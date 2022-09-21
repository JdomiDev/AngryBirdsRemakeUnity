using UnityEngine;
using Assets.Scripts;

public class CameraMove : MonoBehaviour
{
    public CameraFollow cameraFollow;

    public Camera cam;

    private float dragSpeed = 0.01f;
    private float timeDragStarted;
    private Vector3 previousPosition = Vector3.zero;

    public SlingShot SlingShot;


    [SerializeField] private GameObject leftBoundry;
    [SerializeField] private GameObject rightBoundry;

    private void Awake()
    {
        cameraFollow = gameObject.GetComponent<CameraFollow>();

        cam = gameObject.GetComponent<Camera>();
    }
    // Update is called once per frame
    private void Update()
    {


        if (SlingShot.slingshotState != SlingshotState.UserPulling && GameManager.CurrentGameState == GameState.Playing && cameraFollow.IsFollowing == false)
        {

            if (Input.GetMouseButtonDown(0))
            {
                timeDragStarted = Time.time;
                dragSpeed = 0f;
                previousPosition = Input.mousePosition;
            }

            else if (Input.GetMouseButton(0) && Time.time - timeDragStarted > 0.05f)
            {
                float maxSize = gameObject.GetComponent<CameraPinchToZoom>().orthographicSizeMax;
                float minSize = gameObject.GetComponent<CameraPinchToZoom>().orthographicSizeMin;
                float currentSize = cam.orthographicSize;


                float sizeDiff = maxSize - currentSize;


                Vector3 input = Input.mousePosition;
                float deltaX = (previousPosition.x - input.x)  * (dragSpeed / 1.5f) / (4 + (maxSize - cam.orthographicSize));

                float deltaY = (previousPosition.y - input.y) * (dragSpeed / 1.5f) / (4 + (maxSize - cam.orthographicSize));




                float rightXLimit = rightBoundry.transform.position.x / (currentSize - (minSize - 2));
                float leftXLimit = leftBoundry.transform.position.x / (currentSize - (minSize - 2));

                float newX = Mathf.Clamp(transform.position.x + deltaX, leftXLimit, rightXLimit);

                float newY = Mathf.Clamp(transform.position.y + deltaY, sizeDiff * - 1, sizeDiff);

                transform.position = new Vector3(newX, newY, transform.position.z);

                previousPosition = input;
                if(dragSpeed < 0.1f) dragSpeed += 0.002f;
            }
        }
    }
}
