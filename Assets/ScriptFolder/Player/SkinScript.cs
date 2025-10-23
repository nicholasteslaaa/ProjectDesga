using UnityEngine;

public class SkinScript : MonoBehaviour
{
    public PlayerMovement __player;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        float mouseX = mousePosition.x;
        float mouseY = mousePosition.y;

        // Correct diagonal formula for any screen size
        float diagonalY = -((float)Screen.height / Screen.width) * mouseX + Screen.height;

        // Determine which side of diagonal the mouse is on
        int region = (mouseY > diagonalY) ? 1 : 2; // 1 = top-left, 2 = bottom-right

        Vector3 newScale = transform.localScale;

        if (region == 1 && newScale.x < 0)
        {
            __player.getAnimator().Play("SpinningForward");
            newScale.x = 0.5f;
        }
        else if (region == 2 && newScale.x > 0)
        {
            __player.getAnimator().Play("SpinningBackward");
            newScale.x = -0.5f;
        }
        transform.localScale = newScale;
    }

    public Vector3 getScale()
    {
        return transform.localScale;
    }
}
