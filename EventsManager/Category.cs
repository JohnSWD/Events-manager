using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager
{
    public class Category
    {
        private string _typeCategory;

        public string TypeCategory
        {
            get { return _typeCategory; }
            set { _typeCategory = value; }
        }

        public Category() { }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public Category(string typeCategory, int id )
        {
            _typeCategory = typeCategory;
            _id = id;

        }
    }
}
