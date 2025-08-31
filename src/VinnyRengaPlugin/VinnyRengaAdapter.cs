using System;
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
        public VinnyLibDataStructureModel CreateData()
        {
            return new VinnyLibDataStructureModel();
        }

        public void ExportTo(VinnyLibDataStructureModel vinnyData, ImportExportParameters outputParameters)
        {
            VinnyLibConverterKernel.VinnyLibConverter.CreateInstance2().ExportModel(vinnyData, outputParameters);
        }

        internal VinnyLibConverterKernel.VinnyLibConverter mConverter;
    }
}
