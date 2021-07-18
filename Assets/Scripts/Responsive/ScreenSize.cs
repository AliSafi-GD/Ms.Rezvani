using UnityEditor;
using UnityEngine;
public class ScreenSize : MonoBehaviour
{
    private void Start()
    {
        if (Camera.main.aspect >= 1.7)
        {
            Debug.Log("16:9");
            Camera.main.orthographicSize = 5.7f;
        }

        else if (Camera.main.aspect >= 1.6)
        {
            Debug.Log("16:10");
            Camera.main.orthographicSize = 6.3f;
        }

        else if (Camera.main.aspect >= 1.5)
        {
            Debug.Log("3:2");
            Camera.main.orthographicSize = 6.7f;
        }

        else if (Camera.main.aspect >= 1.3)
        {
            Debug.Log("4:3");
            Camera.main.orthographicSize = 7.6f;
        }

        else if (Camera.main.aspect >= 1.2)
        {
            Debug.Log("5:4");
            Camera.main.orthographicSize = 8.1f;
        }


    }
}
