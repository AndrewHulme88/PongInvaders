using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCursor : MonoBehaviour
{
    public Vector3 offset;

    private void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null)
        {
            transform.position = selected.transform.position + offset;
        }
    }
}
