using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager
{
    public class SrlztnTool
    {
        private List<Event> _events;

        public List<Event> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        private List<Category> _categories;

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

    }
}
