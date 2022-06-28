using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using corel = Corel.Interop.VGCore;

namespace Generar_Fuentes
{
    public partial class ControlUI : UserControl
    {
        private corel.Application corelApp;
        private Styles.StylesController stylesController;
        public ControlUI(object app)
        {
            InitializeComponent();
            try
            {
                this.corelApp = app as corel.Application;
                stylesController = new Styles.StylesController(this.Resources, this.corelApp);
            }
            catch
            {
                global::System.Windows.MessageBox.Show("VGCore Error");
            }
            this.corelApp.FrameWork.CommandBars["Standard"].Controls.AddCustomButton("Button", "Si", 0, false);
            btn_Command.Click += (s, e) => { corelApp.MsgShow("Working"); };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            stylesController.LoadThemeFromPreference();
        }

        private void btn_Command_Click(object sender, RoutedEventArgs e)
        {
            FormCarga formularioCarga = new FormCarga();
            formularioCarga.ShowDialog();
            try
            {
                imprimirPalabra(formularioCarga.palabraCargada.Text);
            }
            catch
            {
                MessageBox.MsgShow(this.corelApp, "Algo ha salido mal. Vuelva a intentar por favor.");
            }
        }

        public void imprimirPalabra(string Palabra)
        {
            double posClickX, posClickY;
            int estadoShift;
            int pos = this.corelApp.ActiveDocument.GetUserClick(out posClickX, out posClickY, out estadoShift, 10, false, 0);
            List<string> fuentes = new List<string>() { "Authentic", "Autography", "Brush Script MT", "Freestyle Script", "Harlow Solid Italic", "Lobster 1.4", "Monotype Corsiva", "Charlotte Amalie", "Derlantica Beauty", "Band of Brothers" };

            Layer capa = this.corelApp.ActiveLayer;
            foreach (var fuente in fuentes)
            {
                capa.CreateArtisticText(posClickX, posClickY, Palabra, 0, 0, fuente, 20, cdrTriState.cdrFalse, cdrTriState.cdrFalse, cdrFontLine.cdrNoFontLine, cdrAlignment.cdrCenterAlignment);
            }
            ShapeRange rangoFormas = capa.Shapes.All();
            corel.Rect rectangulo = new corel.Rect();
            rectangulo.Width = 50;
            rectangulo.Height = 50;
            rangoFormas.AlignAndDistribute(cdrAlignDistributeH.cdrAlignDistributeHDistributeSpacing, cdrAlignDistributeV.cdrAlignDistributeVNone, cdrAlignShapesTo.cdrAlignShapesToLastSelected, cdrDistributeArea.cdrDistributeToRect, false, cdrTextAlignOrigin.cdrTextAlignBoundingBox, 0, 0, rectangulo);
        }
    }
}
