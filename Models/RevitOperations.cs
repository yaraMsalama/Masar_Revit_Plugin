using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masarr_Revit_Plugin.Models
{
    public class RevitOperations
    {
        private readonly Document _document;
        private readonly UIDocument _uiDocument;

        public RevitOperations(ExternalCommandData commandData)
        {
            _uiDocument = commandData.Application.ActiveUIDocument;
            _document = _uiDocument.Document;
        }

        public List<ViewPlan> GetStructuralPlans()
        {
            return new FilteredElementCollector(_document)
                .OfClass(typeof(ViewPlan))
                .WhereElementIsNotElementType()
                .Cast<ViewPlan>()
                .Where(v => !v.IsTemplate)
                .ToList();
        }

        public List<FamilySymbol> GetTitleBlocks()
        {
            return new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilySymbol>()
                .ToList();
        }

        public List<Level> GetLevels()
        {
            return new FilteredElementCollector(_document)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();
        }
    }
}
