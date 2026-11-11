using UnityEngine;

public class DesktopWindowManager : MonoBehaviour
{
    public void BringToFront(GameObject window)
    {
        window.transform.SetAsLastSibling();
    }
}