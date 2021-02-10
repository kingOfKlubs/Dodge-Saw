using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabGroup : MonoBehaviour
{
    //credit for original script goes to game dev guide youtube channel

    List<TabButton> tabButtons;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        { 
            button._background.sprite = tabHover;
        }
            
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button._background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].GetComponent<CanvasGroup>().alpha = 1;
                objectsToSwap[i].GetComponent<CanvasGroup>().interactable = true;
                objectsToSwap[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                objectsToSwap[i].GetComponent<CanvasGroup>().alpha = 0;
                objectsToSwap[i].GetComponent<CanvasGroup>().interactable = false;
                objectsToSwap[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

    }
    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab!=null && button == selectedTab) { continue;}
            button._background.sprite = tabIdle;
        }
    }
}
