using UnityEngine;
using Assets.Scripts;


public class CameraPinchToZoom : MonoBehaviour
{
    public float zoomSpeed;
    public float orthographicSizeMin;
    public float orthographicSizeMax;
    private Camera myCamera;


    [SerializeField] private GameObject leftBoundry;
    [SerializeField] private GameObject rightBoundry;

    // Use this for initialization
    private void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        float allowedNegative;
        float allowedPositive;

        allowedNegative = myCamera.orthographicSize - orthographicSizeMax;
        allowedPositive = (myCamera.orthographicSize - orthographicSizeMax) * - 1;
        float currentSize = myCamera.orthographicSize;
        float minSize = orthographicSizeMin;

        if(GameManager.CurrentGameState == GameState.Won || GameManager.CurrentGameState == GameState.Lost)
        {
            myCamera.orthographicSize = orthographicSizeMax;
        }
        else
        {
            // below
            if (transform.position.y <= allowedNegative && transform.position.y != 0)
            {
                transform.position = new Vector3(transform.position.x, allowedNegative, transform.position.z);
            }
            // above
            if (transform.position.y >= allowedPositive && transform.position.y != 0)
            {
                transform.position = new Vector3(transform.position.x, allowedPositive, transform.position.z);
            }


            float rightXLimit = rightBoundry.transform.position.x / (currentSize - (minSize - 2));
            float leftXLimit = leftBoundry.transform.position.x / (currentSize - (minSize - 2));


            if (transform.position.x > rightXLimit)
            {
                transform.position = new Vector3(rightXLimit, transform.position.y, transform.position.z);
            }

            if (transform.position.x < leftXLimit)
            {
                transform.position = new Vector3(leftXLimit, transform.position.y, transform.position.z);
            }

            if (!gameObject.GetComponent<CameraFollow>().IsFollowing)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    myCamera.orthographicSize += zoomSpeed;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    myCamera.orthographicSize -= zoomSpeed;
                }
                myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
            }
        }
    }
}