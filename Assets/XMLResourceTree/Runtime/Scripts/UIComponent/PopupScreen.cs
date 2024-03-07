using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIFramework;
using UnityEngine;

namespace XMLResourceTree
{
    public class PopupScreen : UIScreenBase
    {
        public static PopupScreen singleton;

        private void Awake()
        {
            singleton = this;
        }

        public override string screenName => "PopupScreen";

        protected override async Task OnScreenGoingLeave()
        {
            await Task.CompletedTask;
        }

        protected override async Task OnScreenGoingShow()
        {
            await Task.CompletedTask;
        }

        protected override async Task OnScreenHidden()
        {
            await Task.CompletedTask;
        }

        protected override async Task OnScreenShown()
        {
            await Task.CompletedTask;
        }
    }
}
