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

namespace SPARXEAAutomation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<List<string>> lstRows = getFileContent();
            List<string> lstColumnNames = lstRows.First();
            lstRows = lstRows.Skip(1).ToList();

            EA.Repository repository = new EARepositoryHandler(@"C:\Projects\Sparx EA Projects\testAutomation\testAutomation.eap").Get();

            EA.Package rootModel = new EAPackageHandler(repository).GetByEALocalId(2);
            //Cleanup model
            rootModel.Packages.Cast<EA.Package>().ToList().ForEach(f=>new EAPackageHandler(repository).Delete(f));
            
            EA.Package actorPackage = new EAPackageHandler(repository).Add(rootModel, "Actors", "Package");
            List<string> lstActorsServiceOwners = lstRows.Skip(1).Select(s => s[15].Trim()).Distinct().ToList();
            List<string> lstActorsProcessOwners = lstRows.Skip(1).Select(s => s[16].Trim()).Distinct().ToList();
            List<EA.Element> lstActors = new List<EA.Element>();
            lstActorsServiceOwners.ToList().ForEach(f => lstActors.Add(new EAElementHandler(repository){ StereoType = "Tjänsteägare", Notes = f }.Add(actorPackage, f, EAElementHandler.Type.Actor)));
            lstActorsProcessOwners.ToList().ForEach(f => lstActors.Add(new EAElementHandler(repository) { StereoType = "Processägare", Notes = f }.Add(actorPackage, f, EAElementHandler.Type.Actor)));

            int x = 10; int y = 10;
            foreach (string tjtKat in lstRows.Skip(1).Select(s => s[0].ToString()).Distinct().OrderBy(o => o))
            {
                EA.Package tjtKatPkg =  new EAPackageHandler(repository).Add(rootModel, tjtKat, "Package");
                EA.Diagram dgKatPkg = new EADiagramHandler(repository).Add(tjtKatPkg, tjtKatPkg.Name + "Diagram", "");
                
                foreach (string tjtGrp in lstRows.Skip(1).Where(w => w[0] == tjtKat).Select(s => s[1].ToString()).Distinct().OrderBy(o => o))
                {
                    EA.Package tjtGrpPkg = new EAPackageHandler(repository).Add(tjtKatPkg, tjtGrp, "Package");
                    EA.Diagram dgGrpPkg = new EADiagramHandler(repository).Add(tjtGrpPkg, tjtGrpPkg.Name + "Diagram", "");
                    new EADiagramObjectsHandler(repository).Add(dgKatPkg, tjtGrpPkg);
                    foreach (string tjt in lstRows.Skip(1).Where(w => w[0] == tjtKat).Where(w => w[1] == tjtGrp).Select(s => s[2].ToString()).Distinct().OrderBy(o => o))
                    {
                        EA.Element tjtPkg = new EAElementHandler(repository).Add(tjtGrpPkg, tjt, "Class");
                        string tjänsteÄgare = lstRows.Skip(1).Where(w => w[0] == tjtKat).Where(w => w[1] == tjtGrp).Where(w => w[2] == tjt).Select(s => s[15].Trim()).FirstOrDefault();
                        string processÄgare = lstRows.Skip(1).Where(w => w[0] == tjtKat).Where(w => w[1] == tjtGrp).Where(w => w[2] == tjt).Select(s => s[16].Trim()).FirstOrDefault();
                        if (tjänsteÄgare != "") { new EAConnectionHandler(repository).Add(tjtPkg, lstActors.Where(w=> w.Name == tjänsteÄgare).First(), "Tjänsteägare", EAConnectionHandler.ConnectionTypes.Association); }
                        if (processÄgare != "") { new EAConnectionHandler(repository).Add(tjtPkg, lstActors.Where(w => w.Name == processÄgare).First(), "ProcessÄgare", EAConnectionHandler.ConnectionTypes.Association); }
                        new EADiagramObjectsHandler(repository).Add(dgGrpPkg, tjtPkg, new Point(x, y), new Size(100, 100)); y = y + 100 + 10;
                        if (tjänsteÄgare != "") 
                        {
                            EA.Element actor = lstActors.Where(w => w.Name == tjänsteÄgare).FirstOrDefault();
                            if (actor != null)
                            {
                                if (dgGrpPkg.DiagramObjects.Cast<EA.DiagramObject>().Where(w => w.ElementID == actor.ElementID).Count() == 0)
                                {
                                    new EADiagramObjectsHandler(repository).Add(dgGrpPkg, actor, new Point(x, y), new Size(100, 100)); y = y + 100 + 10;
                                }
                            }
                        }
                        if (processÄgare != "")
                        {
                            EA.Element actor = lstActors.Where(w => w.Name == processÄgare).FirstOrDefault();
                            if (actor != null)
                            {
                                if (dgGrpPkg.DiagramObjects.Cast<EA.DiagramObject>().Where(w => w.ElementID == actor.ElementID).Count() == 0)
                                {
                                    new EADiagramObjectsHandler(repository).Add(dgGrpPkg, actor, new Point(x, y), new Size(100, 100)); y = y + 100 + 10;
                                }
                            }
                        }
                        

                        EA.Diagram dgPkg = new EADiagramHandler(repository).Add(tjtPkg, tjtPkg.Name + "Diagram", "");
                        foreach (string[] prod in lstRows.Skip(1).Where(w => w[8].ToString().Trim() != "").Where(w => w[2] == tjt).Select(s => new string[] { s[8].ToString(), s[9].ToString() }).Distinct())
                        {
                            EA.Attribute prd = new EAAttributeHandler(repository) { Notes = prod[1].Trim() }.Add(tjtPkg, prod[0].Trim(), "Method");
                            new EAElementsUpdaterHandler(prd).DoWork();
                        }
                        new EAElementsUpdaterHandler(tjtPkg).DoWork();
                    }
                    new EAElementsUpdaterHandler(tjtGrpPkg).DoWork();
                }
                new EAElementsUpdaterHandler(tjtKatPkg).DoWork();
            }
            new EAElementsUpdaterHandler(rootModel).DoWork();

            repository.RefreshModelView(0);

            repository.Exit();
        }

        private EA.Package findPackage(EA.Repository repository, string name)
        {
            EA.Package result = null;

            foreach (EA.Package package in repository.Models)
            {
                if (package.Name == name)
                {
                    result = package;
                }
                foreach (EA.Package subPackage in package.Packages)
                {
                    if (subPackage.Name == name)
                    {
                        result = subPackage;
                    }
                }
            }

            return result;
        }

        private List<List<string>> getFileContent()
        {
            List<List<string>> lstResults = new List<List<string>>();
            using (StreamReader read = new StreamReader(@"C:\Projects\SPARXEAAutomation\SPARXEAAutomation\InData\Tjänstekatalog aktuell.csv",Encoding.UTF7))
            {
                List<string> lstFileLines = read.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                lstResults = lstFileLines.Select(s => s.Split(new string[] { ";" }, StringSplitOptions.None).ToList()).Where(w => w.Count() > 0).Where(w => w[0].Trim() != "").Where(w => w[1].Trim() != "").Where(w => w[2].Trim() != "").ToList();
                lstResults.ForEach(f => f.ForEach(f2 => f2 = f2.Trim()));

            }
            return lstResults;
        }

    }
}
