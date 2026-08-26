using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;

namespace LiveSplit.CatQuest3
{
    public class ComponentFactory : IComponentFactory
    {
        public string ComponentName => "Cat Quest III Autosplitter";

        public string Description => "Autosplitter for Cat Quest III";

        public ComponentCategory Category => ComponentCategory.Control;

        public IComponent Create(LiveSplitState state)
        {
            return new CatQuest3Component(state);
        }

        public string UpdateName => ComponentName;

        public string UpdateURL => "";

        public string XMLURL => "";

        public Version Version => new Version(0, 1, 0);
    }
}