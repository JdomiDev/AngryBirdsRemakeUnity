using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargeGui : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cachedScale;

    void Start()
    {
        cachedScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!(transform.localScale.x * 1.1f > cachedScale.x * 1.1f))
        {
            transform.localScale = new Vector3((transform.localScale.x) * 1.1f, (transform.localScale.y) * 1.1f, (transform.localScale.z) * 1.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = cachedScale;
    }

    public void onPointerEnter()
    {
        if(!(transform.localScale.x * 1.1f > cachedScale.x * 1.1f))
        {
            transform.localScale = new Vector3((transform.localScale.x) * 1.1f, (transform.localScale.y) * 1.1f, (transform.localScale.z) * 1.1f);
        }
    }

    public void onPointerExit()
    {
        transform.localScale = cachedScale;
    }

}