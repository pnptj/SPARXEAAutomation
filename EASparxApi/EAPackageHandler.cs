using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EAPackageHandler
    {
        private EA.Repository repository = null;
        private string notes = "";

        public EAPackageHandler(EA.Repository repository) 
        {
            this.repository = repository;
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public EA.Package Add(EA.Package pkg, string name, string type)
        {
            EA.Package result = null;
            result = pkg.Packages.AddNew(name, "");
            result.Notes = this.notes;
            new EAElementsUpdaterHandler(result).DoWork();
            new EAElementsUpdaterHandler(pkg).DoWork();
            pkg.Packages.Refresh();
            return result;
        }

        public List<EA.Package> GetByName(string name)
        {
            List<EA.Package> result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByName(package, name);
            }

            return result;
        }

        private List<EA.Package> getByName(EA.Package package, string name)
        {
            List<EA.Package> result = new List<EA.Package>();

            if (package.Packages.Cast<EA.Package>().Where(w => w.Name == name).Count() > 0)
            {
                result = package.Packages.Cast<EA.Package>().Where(w => w.Name == name).ToList();
            }
            
            foreach (EA.Package subPackage in package.Packages)
            {
                result.AddRange(getByName(subPackage, name));
            }
            return result;
        }

        public EA.Package GetByEALocalId(int id)
        {
            EA.Package result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByEALocalId(package, id);
            }

            return result;
        }

        private EA.Package getByEALocalId(EA.Package package, int id)
        {
            EA.Package result = null;

            if (package.Packages.Cast<EA.Package>().Where(w => w.PackageID == id).Count() > 0)
            {
                result = package.Packages.Cast<EA.Package>().Where(w => w.PackageID == id).FirstOrDefault();
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

        public EA.Package GetById(string id)
        {
            EA.Package result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getById(package, id);
            }

            return result;
        }

        private EA.Package getById(EA.Package package, string id)
        {
            EA.Package result = null;

            if (package.Packages.Cast<EA.Package>().Where(w => w.PackageGUID == id).Count() > 0)
            {
                result = package.Packages.Cast<EA.Package>().Where(w => w.PackageGUID == id).FirstOrDefault();
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

        public void Delete(EA.Package package)
        {
            EA.Package parentPackage = this.GetByEALocalId(package.ParentID);
            short index = 0;// Convert.ToInt16(parentPackage.Packages.Cast<EA.Package>().ToList().IndexOf(package));
            foreach (EA.Package subPackage in parentPackage.Packages)
            {
                if (subPackage.PackageID == package.PackageID)
                {
                    break;
                }
                index++;
            }
            parentPackage.Packages.DeleteAt(index,true);
        }

    }
}
