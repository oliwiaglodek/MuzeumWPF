using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeumInz
{
    public class ExhibitsInExhibitionsDto
    {
        public int exhibitionId;
        public int exhibitId;
        public string ExhibitionName { get; set; }
        public DateTime ExhibitionStart { get; set; }
        public DateTime ExhibitionEnd { get; set; }
        public string ExhibitName { get; set; }

        public ExhibitsInExhibitionsDto(int exhibitionId, int exhibitId, string exhibitionName, DateTime exhibitionStart, DateTime exhibitionEnd, string exhibitName)
        {
            this.exhibitionId = exhibitionId;
            this.exhibitId = exhibitId;
            ExhibitionName = exhibitionName;
            ExhibitionStart = exhibitionStart;
            ExhibitionEnd = exhibitionEnd;
            ExhibitName = exhibitName;
        }
    }
}
