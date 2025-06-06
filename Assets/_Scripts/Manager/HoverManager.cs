using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HoverManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI threatText;
    [SerializeField] TextMeshProUGUI ownerText;
    [SerializeField] TextMeshProUGUI incomeText;
    [SerializeField] RectTransform tipWindow;

    public static Action<Sector, Vector2> onMouseHover;
    public static Action onMouseLoseFocus;  
    private void Start()
    {
        this.hideTip();
    }

    private void OnEnable()
    {
        onMouseHover += this.showTip;
        onMouseLoseFocus += this.hideTip;
    }

    private void OnDisable()
    {
        onMouseHover -= this.showTip;
        onMouseLoseFocus -= this.hideTip;
    }

    private void showTip(Sector sector, Vector2 mousePos)
    {
        if(sector.getData().isRevealed() && GameManager.instance.gameState.Equals(State.PLAYERMOVE))
        {
            this.typeText.text = "Type: " + sector.getData().getType().ToString();
            this.threatText.text = "Threats: "+ sector.getData().getDiff().ToString();
            this.ownerText.text =  "Owner: "+ sector.getData().getOwner().ToString();
            this.incomeText.text = "Income: " + sector.getData().getTaxes().ToString();
            this.tipWindow.gameObject.SetActive(true);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, Input.mousePosition, null, out Vector2 localPos);
            Vector2 offset = new Vector2(this.tipWindow.sizeDelta.x / 2, this.tipWindow.sizeDelta.y / 2.1f);
            this.tipWindow.transform.localPosition = localPos + offset;
        }
    }

    public void hideTip()
    {
        this.typeText.text = default;
        this.threatText.text = default;
        this.ownerText.text = default;
        this.incomeText.text = default;
        this.tipWindow.gameObject.SetActive(false);
    }
}
