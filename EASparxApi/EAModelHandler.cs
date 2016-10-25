using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EAModelHandler
    {
        private EA.Repository repository = null;
        private string notes = "";

        public EAModelHandler(EA.Repository repository)
        {
            this.repository = repository;
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public EA.Package Add(string name)
        {
            EA.Package result = null;
            result = repository.Models.AddNew(name, "Model");
            result.Notes = this.notes;
            new EAElementsUpdaterHandler(result).DoWork();
            repository.Models.Refresh();
            return result;
        }

        public EA.Package GetByName(string name)
        {
            EA.Package result = null;

            foreach (EA.Package package in repository.Models)
            {
                if (package.Name == name)
                {
                    result = package;
                    break;
                }
                foreach (EA.Package subPackage in package.Packages)
                {
                    if (subPackage.Name == name)
                    {
                        result = subPackage;
                        break;
                    }
                }
            }

            return result;
        }

        public EA.Package GetByEALocalId(int id)
        {
            EA.Package result = null;

            foreach (EA.Package package in repository.Models)
            {
                if (package.PackageID == id)
                {
                    result = package;
                    break;
                }
                foreach (EA.Package subPackage in package.Packages)
                {
                    if (subPackage.PackageID == id)
                    {
                        result = subPackage;
                        break;
                    }
                }
            }

            return result;
        }

        public EA.Package GetById(Guid id)
        {
            EA.Package result = null;

            foreach (EA.Package package in repository.Models)
            {
                if (package.PackageGUID == id.ToString())
                {
                    result = package;
                    break;
                }
                foreach (EA.Package subPackage in package.Packages)
                {
                    if (subPackage.PackageGUID == id.ToString())
                    {
                        result = subPackage;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
