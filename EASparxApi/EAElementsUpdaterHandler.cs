using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EAElementsUpdaterHandler
    {
        private EA.Package          package         = null;
        private EA.Element          element         = null;
        private EA.Stereotype       stereotype      = null;
        private EA.Diagram          diagram         = null;
        private EA.Method           method          = null;
        private EA.Attribute        attribute       = null;
        private EA.Parameter        parameter       = null;
        private EA.DiagramObject    diagramObject   = null;
        private EA.Connector        connector       = null;
        private EA.Constraint       constraint = null;
        private EA.TaggedValue      taggedValue = null;

        public EAElementsUpdaterHandler(EA.TaggedValue taggedValue)
        {
            this.taggedValue = taggedValue;
        }

        public EAElementsUpdaterHandler(EA.Constraint constraint)
        {
            this.constraint = constraint;
        }
        
        public EAElementsUpdaterHandler(EA.Connector connector)
        {
            this.connector = connector;
        }

        public EAElementsUpdaterHandler(EA.Package package)
        {
            this.package = package;
        }

        public EAElementsUpdaterHandler(EA.Element element)
        {
            this.element = element;
        }

        public EAElementsUpdaterHandler(EA.Stereotype stereotype)
        {
            this.stereotype = stereotype;
        }


        public EAElementsUpdaterHandler(EA.Diagram diagram)
        {
            this.diagram = diagram;
        }


        public EAElementsUpdaterHandler(EA.Method method)
        {
            this.method = method;
        }


        public EAElementsUpdaterHandler(EA.Attribute attribute)
        {
            this.attribute = attribute;
        }


        public EAElementsUpdaterHandler(EA.Parameter parameter)
        {
            this.parameter = parameter;
        }


        public EAElementsUpdaterHandler(EA.DiagramObject diagramObject)
        {
            this.diagramObject = diagramObject;
        }

        public void DoWork()
        {
            if(package!=null){ Update(package);}
            if (element != null) { Update(element); }
            if (stereotype != null) { Update(stereotype); }
            if (diagram != null) { Update(diagram); }
            if (method != null) { Update(method); }
            if (attribute != null) { Update(attribute); }
            if (parameter != null) { Update(parameter); }
            if (diagramObject != null) { Update(diagramObject); }
            if (connector != null) { Update(connector); }
            if (constraint != null) { Update(constraint); }
            if (taggedValue != null) { Update(taggedValue); }
        }

        private bool Update(EA.TaggedValue tgdVl)
        {
            bool result = tgdVl.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }
 
        private bool Update(EA.Constraint con)
        {
            bool result = con.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }
 
        private bool Update(EA.Connector con)
        {
            bool result = con.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }
        
        private bool Update(EA.Package pkg)
        {
            bool result = pkg.Update();
            pkg.Packages.Refresh();
            pkg.Elements.Refresh();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.Element elm)
        {
            bool result = elm.Update();
            elm.Elements.Refresh();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.Stereotype strTp)
        {
            bool result = strTp.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.Diagram drg)
        {
            bool result = drg.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.Method mth)
        {
            bool result = mth.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.Attribute atr)
        {
            bool result = atr.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.Parameter prm)
        {
            bool result = prm.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }

        private bool Update(EA.DiagramObject dgrO)
        {
            bool result = dgrO.Update();

            if (result == false)
            {
                throw new Exception("Update Didn't work out properlly!!!");
            }
            return result;
        }
    }
}
