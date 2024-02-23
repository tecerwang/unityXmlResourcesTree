using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIFramework;
using UnityEngine;

public class PopupScreen : UIScreenBase
{
    public static PopupScreen singleton;

    private void Awake()
    {
        singleton = this;
    }

    public override string screenName => "PopupScreen";

    public override async Task OnScreenGoingLeave()
    {
        await Task.CompletedTask;
    }

    public override async Task OnScreenGoingShow()
    {
        await Task.CompletedTask;
    }

    public override async Task OnScreenHidden()
    {
        await Task.CompletedTask;
    }

    public override async Task OnScreenShown()
    {
        await Task.CompletedTask;
    }
}
