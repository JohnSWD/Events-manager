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


        /*private int _ageCategory;

        public int AgeCategory
        {
            get { return _ageCategory; }
            set { _ageCategory = value; }
        }*/

        public Category (string typeCategory /*int ageCategory*/)
        {
            _typeCategory = typeCategory;
            //_ageCategory = ageCategory;

        }

    }
}
