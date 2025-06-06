using UnityEngine;
public class CloseCanvas : MonoBehaviour
{
    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && this.gameObject.activeInHierarchy)
        {
            CanvasManager.instance.setTimeFlow(1f);
            CanvasManager.instance.setInOtherMenu(false);
            this.gameObject.SetActive(false);
        }
    }
}
