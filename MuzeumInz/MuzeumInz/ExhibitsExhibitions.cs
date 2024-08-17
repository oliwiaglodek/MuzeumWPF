using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeumInz
{
    public class ExhibitsExhibitions
    {
        public int Id { get; set; }
        public int IdExhibitions { get; set; }
        public int IdExhibits {  get; set; }

        public ExhibitsExhibitions (int id, int idExhibitions, int idExhibits) 
        {
            Id = id;
            IdExhibitions = idExhibitions;
            IdExhibits = idExhibits;
        }
    }
}
