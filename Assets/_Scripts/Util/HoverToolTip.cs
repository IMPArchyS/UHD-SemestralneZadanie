using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverToolTip : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler, IPointerEnterHandler
{
    private Sector s;
    private bool on;
    private void Awake()
    {
        this.s = GetComponentInParent<Sector>();
        this.on = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        if(GameManager.instance.gameState.Equals(State.PLAYERMOVE) && !this.on)
        {
            StartCoroutine(this.wait(0.5f));
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(GameManager.instance.gameState.Equals(State.PLAYERMOVE) && this.on)
            StartCoroutine(this.wait(0));
        else
            HoverManager.onMouseLoseFocus();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverManager.onMouseLoseFocus();
        this.on = false;
    }

    private void showMSG()
    {
        HoverManager.onMouseHover(this.s, Input.mousePosition);
    }

    private IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        this.on = true;
        this.showMSG();
    }
}
