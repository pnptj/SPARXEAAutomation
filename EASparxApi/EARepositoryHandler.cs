using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASparxApi
{
    public class EARepositoryHandler
    {
        private EA.Repository repository = null;
        private string fileName;
        private string notes = "";

        public EARepositoryHandler(string fileName)
        {
            this.fileName = fileName;
        }


        public string Notes
        {
            set { this.notes = value; }
        }

        public EA.Repository Get()
        {
            repository = new EA.Repository();
            try
            {
                 repository.OpenFile(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return repository;
        }


    }
}
