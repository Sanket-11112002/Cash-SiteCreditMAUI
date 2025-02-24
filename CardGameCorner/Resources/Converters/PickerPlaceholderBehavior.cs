using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Resources.Converters
{
    public class PickerPlaceholderBehavior : Behavior<Picker>
    {
        protected override void OnAttachedTo(Picker picker)
        {
            base.OnAttachedTo(picker);
            picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
        }

        protected override void OnDetachingFrom(Picker picker)
        {
            base.OnDetachingFrom(picker);
            picker.SelectedIndexChanged -= OnPickerSelectedIndexChanged;
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedItem == null)
            {
                picker.Title = picker.Title; // Reset to placeholder
            }
        }
    }
}
