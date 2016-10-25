using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EAElementHandler
    {
        private EA.Repository repository = null;
        private string stereoType = null;
        private string notes = "";

        public EAElementHandler(EA.Repository repository) 
        {
            this.repository = repository;
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public string StereoType
        {
            set { this.stereoType = value; }
        }

        public enum Type
        {
            Class = 0,
            Actor = 1,
            Table = 2,
        }

        public EA.Element Add(EA.Package pkg, string name, string type)
        {
            EA.Element result = null;
            result = pkg.Elements.AddNew(name, type);
            result.Notes = this.notes;
            if (stereoType != null) { result.Stereotype = this.stereoType; }
            new EAElementsUpdaterHandler(result).DoWork();
            new EAElementsUpdaterHandler(pkg).DoWork();
            pkg.Packages.Refresh();
            result.Refresh();
            return result;
        }

        public EA.Element Add(EA.Package pkg, string name, Type type)
        {
            EA.Element result = null;
            result = pkg.Elements.AddNew(name, type.ToString());
            result.Notes = this.notes;
            if (stereoType != null) { result.Stereotype = this.stereoType; }
            new EAElementsUpdaterHandler(result).DoWork();
            new EAElementsUpdaterHandler(pkg).DoWork();
            pkg.Packages.Refresh();
            result.Refresh();
            return result;
        }

        public List<EA.Element> GetByName(string name)
        {
            List<EA.Element> result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByName(package, name);
            }

            return result;
        }

        private List<EA.Element> getByName(EA.Package package, string name)
        {
            List<EA.Element> result = new List<EA.Element>();

            if (package.Packages.Cast<EA.Element>().Where(w => w.Name == name).Count() > 0)
            {
                result = package.Packages.Cast<EA.Element>().Where(w => w.Name == name).ToList();
            }

            foreach (EA.Package subPackage in package.Packages)
            {
                result.AddRange(getByName(subPackage, name));
            }
            return result;
        }

        public EA.Element GetByEALocalId(int id)
        {
            EA.Element result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByEALocalId(package, id);
            }

            return result;
        }

        private EA.Element getByEALocalId(EA.Package package, int id)
        {
            EA.Element result = null;

            if (package.Packages.Cast<EA.Element>().Where(w => w.PackageID == id).Count() > 0)
            {
                result = package.Packages.Cast<EA.Element>().Where(w => w.PackageID == id).FirstOrDefault();
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

        public EA.Element GetById(string id)
        {
            EA.Element result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getById(package, id);
            }

            return result;
        }

        private EA.Element getById(EA.Package package, string id)
        {
            EA.Element result = null;

            if (package.Packages.Cast<EA.Element>().Where(w => w.ElementGUID == id).Count() > 0)
            {
                result = package.Packages.Cast<EA.Element>().Where(w => w.ElementGUID == id).FirstOrDefault();
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
