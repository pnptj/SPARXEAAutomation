using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EASparxApi;

namespace SPARXEADatamodel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //C:\Projects\Sparx EA Projects\dataModelAutomation.eap
            List<List<string>> lstRows = getFileContent();
            List<string> lstColumnNames = lstRows.First();
            lstRows = lstRows.Skip(1).ToList();

            EA.Repository repository = new EARepositoryHandler(@"C:\Projects\Sparx EA Projects\dataModelAutomation.eap").Get();

            EA.Package rootModel = new EAPackageHandler(repository).GetByEALocalId(3);
            //Cleanup model
            rootModel.Packages.Cast<EA.Package>().ToList().ForEach(f => new EAPackageHandler(repository).Delete(f));
            List<string> lstEntitiyNames = lstRows.Select(s => s[0]).Distinct().ToList();
            lstEntitiyNames.AddRange(lstRows.Select(s => s[2]).Distinct().ToList());
            lstEntitiyNames = lstEntitiyNames.Distinct().ToList();

            EA.Package entityPackages = new EAPackageHandler(repository).Add(rootModel, "Entities", "Package");
            List<EA.Element> lstEntities = new List<EA.Element>();
            lstEntitiyNames.ForEach(f => lstEntities.Add(new EAElementHandler(repository) { StereoType = "Table" }.Add(entityPackages, f, EAElementHandler.Type.Class)));

            EA.Diagram diagramEntities = new EADiagramHandler(repository).Add(rootModel, "EntitiesDiagram", "");
            lstRows.ForEach(f => new EAConnectionHandler(repository).Add(lstEntities.Where(w => w.Name == f[0]).First(), lstEntities.Where(w => w.Name == f[2]).First(), "", EAConnectionHandler.ConnectionTypes.Association));

            lstEntities.ForEach(f => new EADiagramObjectsHandler(repository).Add(diagramEntities, f));
            
            repository.Exit();

        }


        private List<List<string>> getFileContent()
        {
            List<List<string>> lstResults = new List<List<string>>();
            using (StreamReader read = new StreamReader(@"C:\Projects\SPARXEAAutomation\SPARXEADatamodel\InData\data.csv", Encoding.UTF7))
            {
                List<string> lstFileLines = read.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                lstResults = lstFileLines.Select(s => s.Split(new string[] { ";" }, StringSplitOptions.None).ToList()).Where(w => w.Count() > 0).Where(w => w[0].Trim() != "").Where(w => w[1].Trim() != "").Where(w => w[2].Trim() != "").ToList();
                lstResults.ForEach(f => f.ForEach(f2 => f2 = f2.Trim()));

            }
            return lstResults;
        }
    }
}
