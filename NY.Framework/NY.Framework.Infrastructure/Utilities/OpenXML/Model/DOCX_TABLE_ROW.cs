using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities.OpenXML.Model
{
    public class DOCX_TABLE_ROW
    {
        List<DOCX_TABLE_CELL> _cells { get; set; }

        public DOCX_TABLE_ROW()
        {
            _cells = new List<DOCX_TABLE_CELL>();
        }

        public List<DOCX_TABLE_CELL> getCells()
        {
            return _cells;
        }

        public DOCX_TABLE_CELL Cell(int index)
        {
            return _cells[index];
        }

        public void AddCell(DOCX_TABLE_CELL cell)
        {
            _cells.Add(cell);
        }
    }
}
