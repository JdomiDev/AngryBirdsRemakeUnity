using UnityEngine.EventSystems;
using UnityEngine;

public class EnlargeChild : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject parent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.GetComponent<EnlargeGui>().onPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        parent.GetComponent<EnlargeGui>().onPointerExit();
    }
}
