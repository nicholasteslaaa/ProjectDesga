using UnityEngine;
using UnityEngine.EventSystems;

public class WheelOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered " + gameObject.name);
        // Example: change color
        GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited " + gameObject.name);
        // Example: reset color
        GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }

}
