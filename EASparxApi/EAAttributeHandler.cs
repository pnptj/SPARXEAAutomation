using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EAAttributeHandler
    {
        private EA.Repository repository = null;
        private string notes = "";

        public EAAttributeHandler(EA.Repository repository)
        {
            this.repository = repository;
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public EA.Attribute Add(EA.Element element, string name, string type)
        {
            EA.Attribute result = null;
            result = element.Attributes.AddNew(name, type);
            result.Notes = this.notes;
            new EAElementsUpdaterHandler(result).DoWork();
            new EAElementsUpdaterHandler(element).DoWork();
            element.Methods.Refresh();
            return result;
        }


        public List<EA.Attribute> GetByName(string name)
        {
            List<EA.Attribute> result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByName(package, name);
            }

            return result;
        }

        private List<EA.Attribute> getByName(EA.Package package, string name)
        {
            List<EA.Attribute> result = new List<EA.Attribute>();

            foreach (EA.Element element in package.Elements)
            {
                if (element.Attributes.Cast<EA.Attribute>().Where(w => w.Name == name).Count() > 0)
                {
                    result.AddRange(element.Attributes.Cast<EA.Attribute>().Where(w => w.Name == name).ToList());
                }
            }

            foreach (EA.Package subPackage in package.Packages)
            {
                result.AddRange(getByName(subPackage, name));
            }
            return result;
        }

        public EA.Attribute GetByEALocalId(int id)
        {
            EA.Attribute result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getByEALocalId(package, id);
            }

            return result;
        }

        private EA.Attribute getByEALocalId(EA.Package package, int id)
        {
            EA.Attribute result = null;

            foreach (EA.Element element in package.Elements)
            {
                if (element.Attributes.Cast<EA.Attribute>().Where(w => w.AttributeID == id).Count() > 0)
                {
                    result = element.Attributes.Cast<EA.Attribute>().Where(w => w.AttributeID == id).FirstOrDefault();
                    break;
                }
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

        public EA.Attribute GetById(string id)
        {
            EA.Attribute result = null;

            foreach (EA.Package package in repository.Models)
            {
                result = getById(package, id);
            }

            return result;
        }

        private EA.Attribute getById(EA.Package package, string id)
        {
            EA.Attribute result = null;

            foreach (EA.Element element in package.Elements)
            {
                if (element.Attributes.Cast<EA.Attribute>().Where(w => w.AttributeGUID == id).Count() > 0)
                {
                    result = element.Attributes.Cast<EA.Attribute>().Where(w => w.AttributeGUID == id).FirstOrDefault();
                    break;
                }
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
