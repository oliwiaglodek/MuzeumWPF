using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeumInz
{
    public class AddExhibitions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public AddExhibitions(int id, string name, string description, DateTime? startDate, DateTime? endDate, string location, string responsiblePerson, string status, string type) 
        { 
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            ResponsiblePerson = responsiblePerson;
            Status = status;
            Type = type;
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
