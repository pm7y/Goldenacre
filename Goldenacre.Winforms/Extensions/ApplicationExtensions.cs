using System.Linq;
using System.Windows.Forms;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ApplicationExtensions
    {
        public static Form RootForm(this Application app)
        {
            return Application.OpenForms.Cast<Form>().FirstOrDefault(f =>
                f != null &&
                !f.IsDisposed &&
                !f.Disposing &&
                f.Owner == null &&
                f.Parent == null &&
                f.ParentForm == null &&
                f.TopLevelControl == null
                );
        }
    }
}