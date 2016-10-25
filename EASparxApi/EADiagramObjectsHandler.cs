using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EASparxApi
{
    public class EADiagramObjectsHandler
    {
        private EA.Repository repository = null;
        private string notes = "";

        public EADiagramObjectsHandler(EA.Repository repository)
        {
            this.repository = repository;
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public void Add(EA.Diagram diagram, EA.Element element)
        {
            Add(diagram, element, new Point(0, 0), new Size(100, 100));
        }

        public void Add(EA.Diagram diagram, EA.Element element, Point pnt, Size size)
        {
            int left = pnt.X; int right = pnt.X + size.Width; int top = pnt.Y; int bottom = pnt.Y + size.Height;
            EA.DiagramObject o = diagram.DiagramObjects.AddNew("l=" + left.ToString() + ";r=" + right.ToString() + ";t=" + top.ToString() + ";b=" + bottom.ToString() + ";", "");
            o.ElementID = element.ElementID;
            new EAElementsUpdaterHandler(diagram).DoWork();
            new EAElementsUpdaterHandler(element).DoWork();
            new EAElementsUpdaterHandler(o).DoWork();
            element.Diagrams.Refresh();
            diagram.DiagramObjects.Refresh();
        }

        public void Add(EA.Diagram diagram, EA.Package package)
        {
            Add(diagram, package, new Point(0, 0), new Size(100, 100));
        }

        public void Add(EA.Diagram diagram, EA.Package package, Point pnt, Size size)
        {
            int left = pnt.X; int right = pnt.X + size.Width; int top = pnt.Y; int bottom = pnt.Y + size.Height;
            EA.DiagramObject o = diagram.DiagramObjects.AddNew("l=" + left.ToString() + ";r=" + right.ToString() + ";t=" + top.ToString() + ";b=" + bottom.ToString() + ";", "");
            o.ElementID = package.PackageID;
            new EAElementsUpdaterHandler(diagram).DoWork();
            new EAElementsUpdaterHandler(package).DoWork();
            new EAElementsUpdaterHandler(o).DoWork();
            package.Diagrams.Refresh();
            diagram.DiagramObjects.Refresh();
        }
    }
}
