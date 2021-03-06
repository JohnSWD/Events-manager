﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EventsManager
{
    public class Event
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private string _location;

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }


        private int _price;

        public int Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private string _description;

        public string Descirption
        {
            get { return _description; }
            set { _description = value; }
        }

        private Category _category;
        [XmlIgnore]
        public Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private int _categoryId;

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        private DateTime _eventDate; 

        public DateTime EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; }
        }

        public Event(string name, string location, int price, string description)
        {
            _name = name;
            _location = location;
            _price = price;
            _description = description;
            
        }

        public Event() {}
    }
}
