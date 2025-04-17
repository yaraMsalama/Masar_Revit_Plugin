using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masarr_Revit_Plugin.App.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class AutoGenerator
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;
            try
            {
                using (Transaction transaction = new Transaction(document, "Sheet Generated"))
                {
                    transaction.Start();
                    FilteredElementCollector element = new FilteredElementCollector(document)
                            .OfClass(typeof(FamilySymbol))
                            .OfCategory(BuiltInCategory.OST_TitleBlocks);

                    FamilySymbol fs = element.FirstOrDefault() as FamilySymbol;
                    if (fs == null)
                    {
                        message = "No title block found.";
                        return Result.Failed;
                    }

                    List<ViewPlan> viewCollector = new FilteredElementCollector(document)
                          .OfClass(typeof(ViewPlan))
                          .WhereElementIsNotElementType()
                          .Cast<ViewPlan>()
                          .Where(v => !v.IsTemplate)
                          .ToList();

                    int sheetCounter = 1;
                    foreach (ViewPlan structuralPlan in viewCollector)
                    {
                        ViewSheet newSheet = ViewSheet.Create(document, fs.Id);
                        newSheet.Name = $"Sheet for {structuralPlan.Name}";
                        newSheet.SheetNumber = $"S-{sheetCounter:D2}";

                        BoundingBoxUV outline = newSheet.Outline;
                        UV location = new UV(
                            (outline.Max.U + outline.Min.U) / 2,
                            (outline.Max.V + outline.Min.V) / 2
                        );
                        Viewport.Create(document, newSheet.Id, structuralPlan.Id, new XYZ(location.U, location.V, 0));

                        sheetCounter++;
                    }
                    transaction.Commit();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
