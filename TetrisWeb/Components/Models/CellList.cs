using System.Collections.Generic;
using System.Linq;

namespace TetrisWeb.Components.Models
{
    public class CellList
    {
        private readonly List<Cell> cells = new List<Cell>();

        public List<Cell> GetAll()
        {
            return cells;
        }

        public List<Cell> GetAllInRow(int row)
        {
            return cells.Where(x => x.Row == row).ToList();
        }

        public void Add(int row, int column)
        {
            cells.Add(new Cell(row, column));
        }

        public void AddMany(List<Cell> _cells, string cssClass)
        {
            foreach (var cell in cells)
            {
                cells.Add(new Cell(cell.Row, cell.Column, cssClass));
            }
        }

        public bool Contains(int row, int column)
        {
            return cells.Any(c => c.Row == row && c.Column == column);
        }

        public bool HasColumn(int column)
        {
            return cells.Any(c => c.Column == column);
        }

        public List<Cell> GetLowest()
        {
            List<Cell> _cells = new List<Cell>();
            foreach (var cell in cells)
            {
                if (!Contains(cell.Row - 1, cell.Column))
                {
                    _cells.Add(cell);
                }
            }

            return cells;
        }

        public List<Cell> GetRightmost()
        {
            List<Cell> _cells = new List<Cell>();
            foreach (var cell in cells)
            {
                if (!Contains(cell.Row, cell.Column + 1))
                {
                    _cells.Add(cell);
                }
            }

            return cells;
        }

        public List<Cell> GetLeftmost()
        {
            List<Cell> _cells = new List<Cell>();
            foreach (var cell in cells)
            {
                if (!Contains(cell.Row, cell.Column - 1))
                {
                    _cells.Add(cell);
                }
            }

            return cells;
        }

        public bool HasRow(int row)
        {
            return cells.Any(c => c.Row == row);
        }

        public string GetCssClass(int row, int column)
        {
            var matching = cells.FirstOrDefault(x => x.Row == row && x.Column == column);

            if (matching != null)
                return matching.CssClass;

            return "";
        }

        public void SetCssClass(int row, string cssClass)
        {
            cells.Where(x => x.Row == row).ToList().ForEach(x => x.CssClass = cssClass);
        }

        public void CollapseRows(List<int> rows)
        {
            var selectedCells = cells.Where(x => rows.Contains(x.Row));

            List<Cell> toRemove = new List<Cell>();
            foreach (var cell in selectedCells)
            {
                toRemove.Add(cell);
            }

            cells.RemoveAll(x => toRemove.Contains(x));

            foreach (var cell in cells)
            {
                int numberOfLessRows = rows.Where(x => x <= cell.Row).Count();
                cell.Row -= numberOfLessRows;
            }
        }
    }
}
