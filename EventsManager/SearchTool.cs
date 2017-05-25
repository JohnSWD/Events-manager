using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

namespace EventsManager
{
    public class SearchTool
    {
        public SearchTool (ICollectionView icv, TextBox textbox)
        {
            string filterText = "";
            icv.Filter = obj =>
            {
                if (string.IsNullOrEmpty(filterText))
                    return true;
                var dynamicItem = obj as dynamic;
                if (string.IsNullOrEmpty(dynamicItem.Name))
                    return false;
                
                int index = dynamicItem.Name.IndexOf(filterText, 0, StringComparison.InvariantCultureIgnoreCase);
                return index > -1;
            };

            textbox.TextChanged += delegate
            {
                filterText = textbox.Text;
                icv.Refresh();
            };
        }
    }
}
