using System;
using System.Collections.Generic;

namespace bashforms.widgets.controls
{
    public interface IOptionGroup {
        void Register(Option option);
        void Unregister(Option option);

        void ReportSelection(Option option);
    }
    
    
    public class SingleOptionGroup : IOptionGroup
    {
        private readonly List<Option> _options = new List<Option>();
        
        public void Register(Option option) {
            _options.Add(option);
        }

        public void Unregister(Option option) {
            _options.Remove(option);
        }

        public void ReportSelection(Option selected) {
            Selection = selected;
            foreach (var option in _options)
                option.Selected = option == selected;
            this.OnSelected(selected,new EventArgs());
        }

        public Option Selection { get; private set; }

        
        public event Action<Option, EventArgs> OnSelected = (sender, args) => { };
    }
}