using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EAConnectionHandler
    {
        private EA.Repository repository = null;
        private string notes = "";

        public EAConnectionHandler(EA.Repository repository) 
        {
            this.repository = repository;
        }

        public enum ConnectionTypes
        {
            Association,
            Realization,
        }

        public string Notes
        {
            set { this.notes = value; }
        }

        public EA.Connector Add(EA.Element source, EA.Element target, string name, ConnectionTypes type)
        {
            EA.Connector result = source.Connectors.AddNew(name, type.ToString());
            result.SupplierID = target.ElementID;
            new EAElementsUpdaterHandler(result).DoWork();
            source.Connectors.Refresh();
            target.Connectors.Refresh();

            return result;
        }
    }
}
