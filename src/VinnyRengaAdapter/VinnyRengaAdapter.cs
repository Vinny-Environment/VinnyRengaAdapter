using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VinnyLibConverterCommon.Interfaces;
using VinnyLibConverterCommon.VinnyLibDataStructure;
using VinnyLibConverterCommon;


namespace VinnyRengaAdapter
{
    public class VinnyRengaAdapter : ICadExportProcessing
    {
        public VinnyRengaAdapter()
        {

        }
        public static VinnyRengaAdapter CreateInstance()
        {
            if (mInstance == null) mInstance = new VinnyRengaAdapter();
            if (mConverter == null) mConverter = VinnyLibConverterKernel.VinnyLibConverter.CreateInstance2();
            return mInstance;
        }
        public void Start()
        {
            VinnyLibConverterUI.VLC_UI_MainWindow vinnyWindow = new VinnyLibConverterUI.VLC_UI_MainWindow(false);
            //WindowInteropHelper win = new WindowInteropHelper(vinnyWindow);
            //win.Owner = rengaApp.GetMainWindowHandle();
            if (vinnyWindow.ShowDialog() == true)
            {
                ExportTo(CreateData(), vinnyWindow.VinnyParametets);
            }
        }
        public VinnyLibDataStructureModel CreateData()
        {
            return new VinnyLibDataStructureModel();
        }

        public void ExportTo(VinnyLibDataStructureModel vinnyData, ImportExportParameters outputParameters)
        {
            mConverter.ExportModel(vinnyData, outputParameters);
        }

        private static VinnyRengaAdapter mInstance;
        private static VinnyLibConverterKernel.VinnyLibConverter mConverter;
    }
}
