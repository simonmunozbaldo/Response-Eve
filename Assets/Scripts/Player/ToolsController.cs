using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsController:MonoBehaviour
{
    public Transform toolsParent;
    public Tool currentActiveTool = null;

    private List<Tool> tools;
    private Scope scope;
    private string lastTag;
    private void Awake()
    {
        scope = GameObject.Find("Scope").GetComponent<Scope>();
        tools = new List<Tool>();
        for(int i = 0; i < toolsParent.childCount; i++)
        {
            tools.Add(toolsParent.GetChild(i).GetComponent<Tool>());
        }
    }


    public Tool GetToolByTag(string tag)
    {
        foreach(Tool t in tools)
        {
            if (t.tagName == tag)
            {
                return t;
            }
        }
        return null;
    }

    public void SwitchTool(string tag)
    {
        if (lastTag == tag)
        {
            return;
        }

        lastTag = tag;

        //Va a cambiar de tool
        if (currentActiveTool != null)
        {
            currentActiveTool.gameObject.SetActive(false);
        }

        Tool tool = GetToolByTag(tag);
        scope.ChangeColor(tag, tool);

        if (tool == null)
        {
            currentActiveTool = null;
            return;
        }

        tool.gameObject.SetActive(true);
        currentActiveTool = tool;
    }

    public void UpdateCurrentWeapon()
    {
        if(currentActiveTool != null)
        {
            currentActiveTool.gameObject.SetActive(false);
            currentActiveTool.gameObject.SetActive(true);
        }
    }
}
