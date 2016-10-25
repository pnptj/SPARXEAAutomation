using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EADiagramHandler
    {
        private EA.Repository repository = null;
        private string notes = "";

        public EADiagramHandler(EA.Repository repository) 
        {
            this.repository = repository;
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public EA.Diagram Add(EA.Package pkg, string name, string type)
        {
            EA.Diagram result = null;
            result = pkg.Diagrams.AddNew(name, type);
            result.Notes = this.notes;
            new EAElementsUpdaterHandler(result).DoWork();
            new EAElementsUpdaterHandler(pkg).DoWork();
            pkg.Diagrams.Refresh();
            return result;
        }

        public EA.Diagram Add(EA.Element element, string name, string type)
        {
            EA.Diagram result = null;
            result = element.Diagrams.AddNew(name, type);
            result.Notes = this.notes;
            new EAElementsUpdaterHandler(result).DoWork();
            new EAElementsUpdaterHandler(element).DoWork();
            element.Diagrams.Refresh();
            return result;
        }

        public List<EA.Diagram> GetByName(string name)
        {
            List<EA.Diagram> result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByName(package, name);
            }

            return result;
        }

        private List<EA.Diagram> getByName(EA.Package package, string name)
        {
            List<EA.Diagram> result = new List<EA.Diagram>();

            if (package.Packages.Cast<EA.Diagram>().Where(w => w.Name == name).Count() > 0)
            {
                result = package.Packages.Cast<EA.Diagram>().Where(w => w.Name == name).ToList();
            }

            foreach (EA.Package subPackage in package.Packages)
            {
                result.AddRange(getByName(subPackage, name));
            }
            return result;
        }

        public EA.Diagram GetByEALocalId(int id)
        {
            EA.Diagram result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByEALocalId(package, id);
            }

            return result;
        }

        private EA.Diagram getByEALocalId(EA.Package package, int id)
        {
            EA.Diagram result = null;

            if (package.Packages.Cast<EA.Diagram>().Where(w => w.PackageID == id).Count() > 0)
            {
                result = package.Packages.Cast<EA.Diagram>().Where(w => w.PackageID == id).FirstOrDefault();
            }

            if (result == null)
            {
                foreach (EA.Package subPackage in package.Packages)
                {
                    result = getByEALocalId(subPackage, id);
                }
            }
            return result;
        }

        public EA.Diagram GetById(string id)
        {
            EA.Diagram result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getById(package, id);
            }

            return result;
        }

        private EA.Diagram getById(EA.Package package, string id)
        {
            EA.Diagram result = null;

            if (package.Packages.Cast<EA.Diagram>().Where(w => w.DiagramGUID == id).Count() > 0)
            {
                result = package.Packages.Cast<EA.Diagram>().Where(w => w.DiagramGUID == id).FirstOrDefault();
            }

            if (result == null)
            {
                foreach (EA.Package subPackage in package.Packages)
                {
                    result = getById(subPackage, id);
                }
            }
            return result;
        }

    }
}
