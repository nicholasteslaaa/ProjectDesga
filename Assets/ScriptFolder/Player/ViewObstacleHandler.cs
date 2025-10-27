using UnityEngine;
using System.Collections.Generic;

public class ViewObstacleHandler : MonoBehaviour
{
    private List<GameObject> insideObjects = new List<GameObject>();
    public float fadeThreshold = 0.3f;

    void OnTriggerEnter(Collider other)
    {
        if (!insideObjects.Contains(other.gameObject))
        {
            FadeObject(other.gameObject, fadeThreshold);
            insideObjects.Add(other.gameObject);
            Debug.Log("Added: " + other.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (insideObjects.Contains(other.gameObject))
        {
            FadeObject(other.gameObject, 1f); // fully visible again
            insideObjects.Remove(other.gameObject);
            Debug.Log("Removed: " + other.name);
        }
    }

    void FadeObject(GameObject obj, float alphaValue)
    {
        // Fade all SpriteRenderers (2D)
        SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in sprites)
        {
            Color c = sr.color;
            c.a = alphaValue;
            sr.color = c;
        }

        // Fade all Renderers (3D objects)
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            Material mat = rend.material;
            if (mat != null)
            {
                Color c = mat.color;
                c.a = alphaValue;
                mat.color = c;
            }
        }
    }


}
