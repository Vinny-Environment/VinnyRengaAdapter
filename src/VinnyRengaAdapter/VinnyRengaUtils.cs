using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinnyLibConverterCommon.VinnyLibDataStructure;

namespace VinnyRengaAdapter
{
    internal static class VinnyRengaUtils
    {
        public static void GetPropertyValueAndTupe(Renga.IProperty _property, out object propValue, out VinnyLibDataStructureParameterDefinitionType propType)
        {
            propValue = null;
            propType = VinnyLibDataStructureParameterDefinitionType.ParamString;
            switch (_property.Type)
            {
                case Renga.PropertyType.PropertyType_Angle:
                    propValue = _property.GetAngleValue(Renga.AngleUnit.AngleUnit_Degrees);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.PropertyType.PropertyType_Area:
                    propValue = _property.GetAreaValue(Renga.AreaUnit.AreaUnit_Meters2);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.PropertyType.PropertyType_Boolean:
                    propValue = _property.GetBooleanValue();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamBool;
                    break;
                case Renga.PropertyType.PropertyType_Double:
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    propValue = _property.GetDoubleValue();
                    break;
                case Renga.PropertyType.PropertyType_Enumeration:
                    propValue = _property.GetEnumerationValue(); break;
                case Renga.PropertyType.PropertyType_Integer:
                    propValue = _property.GetIntegerValue();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamInteger;
                    break;
                case Renga.PropertyType.PropertyType_Length:
                    propValue = _property.GetLengthValue(Renga.LengthUnit.LengthUnit_Meters);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.PropertyType.PropertyType_Logical:
                    propValue = _property.GetLogicalValue(); break;
                case Renga.PropertyType.PropertyType_Mass:
                    propValue = _property.GetMassValue(Renga.MassUnit.MassUnit_Kilograms);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.PropertyType.PropertyType_String:
                    propValue = _property.GetStringValue(); break;
                case Renga.PropertyType.PropertyType_Volume:
                    propValue = _property.GetVolumeValue(Renga.VolumeUnit.VolumeUnit_Meters3);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
            }
        }

        public static void GetQuatityValueAndType(Renga.IQuantity rengaQuantity, out object propValue, out VinnyLibDataStructureParameterDefinitionType propType)
        {
            propValue = null;
            propType = VinnyLibDataStructureParameterDefinitionType.ParamString;

            switch (rengaQuantity.Type)
            {
                case Renga.QuantityType.QuantityType_Count:
                    propValue = rengaQuantity.AsCount();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamInteger;
                    break;
                case Renga.QuantityType.QuantityType_Length:
                    propValue = rengaQuantity.AsLength(Renga.LengthUnit.LengthUnit_Meters);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.QuantityType.QuantityType_Mass:
                    propValue = rengaQuantity.AsMass(Renga.MassUnit.MassUnit_Kilograms);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.QuantityType.QuantityType_Area:
                    propValue = rengaQuantity.AsArea(Renga.AreaUnit.AreaUnit_Meters2);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.QuantityType.QuantityType_Volume:
                    propValue = rengaQuantity.AsVolume(Renga.VolumeUnit.VolumeUnit_Meters3);
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.QuantityType.QuantityType_Unknown:
                    if (rengaQuantity.HasValue()) propValue = rengaQuantity.ToString();
                    break;
            }
        }

        public static void GetParameterValueAndType(Renga.IParameter rengaParameter, out object propValue, out VinnyLibDataStructureParameterDefinitionType propType)
        {
            propValue = null;
            propType = VinnyLibDataStructureParameterDefinitionType.ParamString;

            switch(rengaParameter.ValueType)
            {
                case Renga.ParameterValueType.ParameterValueType_Bool:
                    if (rengaParameter.HasValue)  propValue = rengaParameter.GetBoolValue();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamBool;
                    break;
                case Renga.ParameterValueType.ParameterValueType_Double:
                    if (rengaParameter.HasValue)  propValue = rengaParameter.GetDoubleValue();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamReal;
                    break;
                case Renga.ParameterValueType.ParameterValueType_String:
                    if (rengaParameter.HasValue)  propValue = rengaParameter.GetStringValue();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamString;
                    break;
                case Renga.ParameterValueType.ParameterValueType_Int:
                    if (rengaParameter.HasValue) propValue = rengaParameter.GetIntValue();
                    propType = VinnyLibDataStructureParameterDefinitionType.ParamInteger;
                    break;
            }
        }

        public class RengaQuantityIds
        {
            public static Guid Area { get; } = new Guid(39800047, 31597, 17881, 153, 230, 44, 216, 81, 48, 110, 3);
            public static Guid Count { get; } = new Guid(3398899140U, 24849, 20407, 161, 89, 36, 5, 245, 160, 187, 146);
            public static Guid CrossSectionOverallHeight { get; } = new Guid(3253098413U, 40446, 19223, 132, 220, 90, 8, 2, 73, 133, 54);
            public static Guid CrossSectionOverallWidth { get; } = new Guid(693295390U, 48216, 19092, 131, 43, 245, 131, 194, 46, 251, 68);
            public static Guid GlazingArea { get; } = new Guid(300015282, 8240, 20279, 182, 51, 58, 122, 225, 98, 67, 122);
            public static Guid GrossArea { get; } = new Guid(1773562932U, 54289, 16941, 189, 74, 202, 195, 246, 132, 111, 216);
            public static Guid GrossCeilingArea { get; } = new Guid(1045665218, 24427, 19800, 131, 73, 20, 25, 162, 154, 71, 185);
            public static Guid GrossCrossSectionArea { get; } = new Guid(3012510262U, 5722, 17835, 139, 150, 236, 1, 117, 52, 52, 4);
            public static Guid GrossFloorArea { get; } = new Guid(2309725015U, 37297, 20298, 154, 69, 156, 147, 88, 130, 35, 29);
            public static Guid GrossMass { get; } = new Guid(3765756555U, 1335, 20023, 132, 90, 199, 49, 88, 67, 238, 115);
            public static Guid GrossPerimeter { get; } = new Guid(3948664644U, 15488, 18082, 180, 145, 44, 164, 241, 29, 209, 61);
            public static Guid GrossSideArea { get; } = new Guid(256394U, 41719, 17527, 137, 201, 62, 158, 246, 83, 59, 227);
            public static Guid GrossSideAreaLeft { get; } = new Guid(647920449, 15833, 17105, 176, 208, 7, 78, 137, 163, 162, 131);
            public static Guid GrossSideAreaRight { get; } = new Guid(2620712894U, 10231, 18768, 165, 29, 220, 45, 93, 168, 243, 232);
            public static Guid GrossVolume { get; } = new Guid(3661754522U, 3586, 16583, 149, 71, 42, 15, 85, 182, 0, 120);
            public static Guid GrossWallArea { get; } = new Guid(1095014205U, 50359, 20241, 171, 72, 135, 148, 201, 190, 174, 67);
            public static Guid InnerSurfaceArea { get; } = new Guid(2781515259U, 5062, 16524, 161, 192, 97, 230, 112, 38, 179, 107);
            public static Guid InnerSurfaceExternalArea { get; } = new Guid(2053389762, 21796, 18589, 151, 179, 112, 17, 220, 191, byte.MaxValue, 72);
            public static Guid InnerSurfaceInternalArea { get; } = new Guid(302097120U, 49079, 19240, 136, 246, 146, 53, 113, 151, 40, 144);
            public static Guid NetArea { get; } = new Guid(178998192, 17989, 18646, 157, 203, 42, 202, 72, 87, 126, 71);
            public static Guid NetCeilingArea { get; } = new Guid(3861273692U, 30354, 18877, 183, 161, 10, 95, 69, 47, 238, 221);
            public static Guid NetCrossSectionArea { get; } = new Guid(1595866743, 24110, 17674, 164, 38, 239, 239, 91, 52, 110, 204);
            public static Guid NetFloorArea { get; } = new Guid(3932214566U, 46375, 18582, 142, 76, 200, 74, 132, 98, 179, 204);
            public static Guid NetFootprintArea { get; } = new Guid(1707028630U, 46608, 20400, 182, 3, 158, 31, 213, 194, 16, 149);
            public static Guid NetMass { get; } = new Guid(4179823954U, 15757, 17136, 180, 152, 95, 128, 55, 97, 12, 1);
            public static Guid NetPerimeter { get; } = new Guid(895522956U, 51718, 16424, 168, 33, 114, 107, 171, 3, 133, 54);
            public static Guid NetSideArea { get; } = new Guid(1835608902U, 64626, 18070, 165, 94, 29, 52, 105, 170, 157, 142);
            public static Guid NetSideAreaLeft { get; } = new Guid(22050505, 16448, 17824, 135, 190, 250, 65, 36, 246, 205, 78);
            public static Guid NetSideAreaRight { get; } = new Guid(602395427U, 64553, 19206, 140, 231, 27, 205, 42, 32, 240, 41);
            public static Guid NetVolume { get; } = new Guid(70517235, 2863, 16426, 172, 160, 130, 100, 54, 130, 36, 5);
            public static Guid NetWallArea { get; } = new Guid(3264699934U, 12947, 19153, 129, 54, 49, 109, 25, 27, 117, 204);
            public static Guid NominalArea { get; } = new Guid(4214559128U, 6735, 18453, 149, 62, 23, 124, 23, 231, 100, 28);
            public static Guid NominalHeight { get; } = new Guid(3908150805U, 15116, 16412, 173, 31, 117, 38, 242, 240, 115, 226);
            public static Guid NominalLength { get; } = new Guid(1160713699, 20704, 19878, 145, 85, 86, 122, 0, 210, 213, 183);
            public static Guid NominalThickness { get; } = new Guid(3611768744U, 57294, 18790, 176, 68, 44, 32, 174, 159, 82, 251);
            public static Guid NominalWidth { get; } = new Guid(397447280U, 40958, 19231, 188, 201, 108, 178, 63, 177, 214, 236);
            public static Guid NumberOfRiser { get; } = new Guid(3775213256U, 48126, 18976, 176, 147, 194, 117, 38, 19, 174, 128);
            public static Guid NumberOfTreads { get; } = new Guid(1555579998U, 34115, 18506, 180, 186, 15, 51, 67, 170, 31, 58);
            public static Guid OuterSurfaceArea { get; } = new Guid(3726320167U, 12557, 18002, 167, 147, 199, 153, 197, 190, 112, 54);
            public static Guid OverallDepth { get; } = new Guid(2450563857U, 41720, 18241, 170, 81, 191, 4, 174, 77, 132, 37);
            public static Guid OverallHeight { get; } = new Guid(4284344833U, 31386, 17531, 149, 66, 20, 182, 81, 221, 132, 132);
            public static Guid OverallLength { get; } = new Guid(863916344U, 38809, 17871, 148, 243, 95, 182, 180, 174, 61, 229);
            public static Guid OverallWidth { get; } = new Guid(3310361245U, 12137, 18832, 187, 242, 191, 218, 175, 110, 22, 218);
            public static Guid Perimeter { get; } = new Guid(738665092, 4910, 17472, 129, 120, 224, 79, 100, 183, 51, 32);
            public static Guid ReinforcementUnitCount { get; } = new Guid(2444019173U, 50808, 16932, 170, 29, 245, 105, 169, 229, 236, 191);
            public static Guid RelativeObjectBaselineBottomElevation { get; } = new Guid(2269492940U, 64413, 16753, 132, 193, 63, 240, 216, 142, 174, 22);
            public static Guid RelativeObjectBaselineTopElevation { get; } = new Guid(1016979623U, 48745, 17399, 158, 180, 114, 178, 49, 53, 2, 244);
            public static Guid RelativeObjectBottomElevation { get; } = new Guid(3783098459U, 356, 18442, 132, 155, 100, 253, 2, 46, 193, 122);
            public static Guid RelativeObjectTopElevation { get; } = new Guid(1017741880U, 41653, 16970, 165, 121, 106, 13, 102, 239, 7, 90);
            public static Guid RiserHeight { get; } = new Guid(2265076431U, 59333, 19351, 164, 116, 190, 31, 204, 223, 221, 70);
            public static Guid SheetCount { get; } = new Guid(3963183412U, 21186, 17703, 144, 234, 70, 41, 153, 240, 47, 28);
            public static Guid SheetNumber { get; } = new Guid(1276769563U, 38706, 19786, 149, 109, 206, 10, 186, 18, 133, 157);
            public static Guid SlopeAngle { get; } = new Guid(1698659686, 22672, 17005, 168, 157, 19, 171, 13, 122, 28, 193);
            public static Guid SumConductorsLengths { get; } = new Guid(1720099672U, 39249, 19810, 164, 79, 154, 16, 250, 24, 98, 142);
            public static Guid TotalRebarLength { get; } = new Guid(428864160, 7182, 18440, 137, 69, 115, 79, 111, 131, 220, 231);
            public static Guid TotalRebarMass { get; } = new Guid(4059194287U, 14863, 18700, 137, 70, 126, 31, 206, 50, 129, 121);
            public static Guid TotalSurfaceArea { get; } = new Guid(1672454322, 15812, 17664, 156, 165, 244, 111, 192, 174, 147, 90);
            public static Guid TreadLength { get; } = new Guid(97028042U, 46030, 17714, 129, 48, 131, 109, 8, 28, 146, 191);
            public static Guid Volume { get; } = new Guid(1851983245, 2739, 19133, 169, 186, 87, 78, 23, 70, 197, 173);
            /// <summary>
            /// Типы расчетных свойств для всех категория объектов
            /// </summary>
            /// <returns></returns>
            public static Dictionary<string, Guid> QuantityIdentifiers_Objects()
            {
                return new Dictionary<string, Guid>
                {
                    {"NominalThickness",NominalThickness},
                    {"NominalLength",NominalLength},
                    {"NominalWidth",NominalWidth},
                    {"NominalHeight",NominalHeight},
                    {"Perimeter",Perimeter},
                    {"OverallWidth",OverallWidth},
                    {"OverallHeight",OverallHeight},
                    {"OverallDepth",OverallDepth},
                    {"OverallLength",OverallLength},
                    {"Volume",Volume},
                    {"NetVolume",NetVolume},
                    {"NetMass",NetMass},
                    {"OuterSurfaceArea",OuterSurfaceArea},
                    {"CrossSectionOverallWidth",CrossSectionOverallWidth},
                    {"CrossSectionOverallHeight",CrossSectionOverallHeight},
                    {"NetCrossSectionArea",NetCrossSectionArea},
                    {"GrossCrossSectionArea",GrossCrossSectionArea},
                    {"GrossWallArea",GrossWallArea},
                    {"GrossCeilingArea",GrossCeilingArea},
                    {"Area",Area},
                    {"NominalArea",NominalArea},
                    {"NetArea",NetArea},
                    {"NetFootprintArea",NetFootprintArea},
                    {"NetFloorArea",NetFloorArea},
                    {"NetSideArea",NetSideArea},
                    {"NetPerimeter",NetPerimeter},
                    {"NetWallArea",NetWallArea},
                    {"NetCeilingArea",NetCeilingArea},
                    {"InnerSurfaceArea",InnerSurfaceArea},
                    {"InnerSurfaceInternalArea",InnerSurfaceInternalArea},
                    {"InnerSurfaceExternalArea",InnerSurfaceExternalArea},
                    {"GlazingArea",GlazingArea},
                    {"TotalSurfaceArea",TotalSurfaceArea},
                    {"GrossArea",GrossArea},
                    {"GrossPerimeter",GrossPerimeter},
                    {"GrossFloorArea",GrossFloorArea},
                    {"GrossVolume",GrossVolume},
                    {"NumberOfRiser",NumberOfRiser},
                    {"NumberOfTreads",NumberOfTreads},
                    {"RiserHeight",RiserHeight},
                    {"TreadLength",TreadLength},
                    {"TotalRebarLength",TotalRebarLength},
                    {"TotalRebarMass",TotalRebarMass},
                    {"RelativeObjectBottomElevation",RelativeObjectBottomElevation},
                    {"RelativeObjectTopElevation",RelativeObjectTopElevation},
                    {"RelativeObjectBaselineBottomElevation",RelativeObjectBaselineBottomElevation},
                    {"RelativeObjectBaselineTopElevation",RelativeObjectBaselineTopElevation},
                    {"SlopeAngle",SlopeAngle}
                };
            }
        }

        public class RengaObjectTypes
        {
            public static Guid AngularDimension = new Guid("{96788994-b7fc-41d7-8a99-d674543e9237}");
            public static Guid AssemblyInstance = new Guid("{00799249-1824-4ebd-bf93-40bb92efa9e6}");
            public static Guid Axis = new Guid("{4b41ccf8-c969-4c55-a1f2-cced9c164f07}");
            public static Guid Beam = new Guid("{63478188-7c88-4a6d-b891-9725f04a5bc7}");
            public static Guid Column = new Guid("{d9ee2442-e807-42fb-8fe5-9dcfe543035d}");
            public static Guid DiametralDimension = new Guid("{2aabe3a4-a29e-4534-a9f5-0f070fee240c}");
            public static Guid Door = new Guid("{1cfba99c-01e7-4078-ae1a-3e2ff0673599}");
            public static Guid Duct = new Guid("{06cc88ee-9a67-4626-9c34-dde03c331a74}");
            public static Guid DuctAccessory = new Guid("{47d0d93f-3c7b-4269-bf8a-de246e1724d0}");
            public static Guid DuctFitting = new Guid("{77ffca60-b20e-49f0-b42f-4fdc9b1c825b}");
            public static Guid ElectricDistributionBoard = new Guid("{96da9155-43c1-42b8-bba2-b4f61fa43acc}");
            public static Guid Element = new Guid("{e1e3bd66-2e13-4fa4-a9eb-677e03067c2f}");
            public static Guid Elevation = new Guid("{8a49a9a8-a401-4ab1-8038-92093503c97a}");
            public static Guid Equipment = new Guid("{5d2f3734-5a49-4504-90b1-0676f0f25da7}");
            public static Guid Floor = new Guid("{f5bd8bd8-39c1-47f8-8499-f673c580dfbe}");
            public static Guid Hatch = new Guid("{84b43087-d4a4-4cce-b34d-40e283d9e691}");
            public static Guid Hole = new Guid("{0xecef8f90, 0xcf9, 0x4494, {0x98, 0xde, 0x91, 0x24, 0x2a, 0x2a, 0x9f, 0x5c}}");
            public static Guid IfcObject = new Guid("{f914251d-d5fa-48b2-b93b-074f442cbf3b}");
            public static Guid IsolatedFoundation = new Guid("{6063816c-89ff-4c8f-a814-3be6cb94128e}");
            public static Guid Level = new Guid("{c3ce17ff-6f28-411f-b18d-74fe957b2ba8}");
            public static Guid LightingFixture = new Guid("{793d3f7c-905d-4d85-a351-b152241dd2e7}");
            public static Guid Line3D = new Guid("{02bbebe8-e28b-4ee5-8916-11b514a35dca}");
            public static Guid LinearDimension = new Guid("{dc82ca1a-a0c3-4a1a-aefb-a7d720dd3a09}");
            public static Guid LineElectricalCircuit = new Guid("{83de45e6-4793-49ec-8b9e-65a2438f36de}");
            public static Guid MechanicalEquipment = new Guid("{de4420ce-02b6-4b12-9cd7-9322118be8fe}");
            public static Guid Opening = new Guid("{fc443d5a-b76c-45e5-b91c-520ef0896109}");
            public static Guid Pipe = new Guid("{838cc9f6-e3d8-4132-af6f-c58df0f8d037}");
            public static Guid PipeAccessory = new Guid("{41e2788a-49ed-487f-9ae1-55b6e09ae6e5}");
            public static Guid PipeFitting = new Guid("{d31dc2e3-808e-4987-8481-7f86665a07fc}");
            public static Guid Plate = new Guid("{62cf086e-5a39-4484-840c-ffa6a1c6e2b7}");
            public static Guid PlumbingFixture = new Guid("{b8c7155a-b462-4ff5-bc41-c9c17a9f48fa}");
            public static Guid RadialDimension = new Guid("{377c2fda-9411-43ac-a6c6-0e3b520be721}");
            public static Guid Railing = new Guid("{a1aca786-78a4-4015-b412-9150baad71a9}");
            public static Guid Ramp = new Guid("{debde004-afcc-4da8-8dd0-4223ff836acd}");
            public static Guid Rebar = new Guid("{9fabc932-590f-4068-89a8-ee6ee3d7cbbf}");
            public static Guid Roof = new Guid("{bac4470f-d560-4f57-a49e-faa5f6e5a279}");
            public static Guid Room = new Guid("{f1a805ff-573d-f46b-ffba-57f4bccaa6ed}");
            public static Guid Route = new Guid("{8b323bee-3882-4744-8838-24f45df714a9}");
            public static Guid RoutePoint = new Guid("{ce93e320-7167-4cd1-92a8-5e42d546066b}");
            public static Guid Section = new Guid("{4166fd59-64c0-45ee-ae3b-49fae1257ef1}");
            public static Guid Stair = new Guid("{3f522f49-aee2-4d73-9866-9b07cf336a69}");
            public static Guid TextObject = new Guid("{da557027-f243-4331-bb5b-853abc437cd7}");
            public static Guid Undefined = new Guid("{97675473-ca62-4ea4-bc6e-bb2ca57b7e67}");
            public static Guid Wall = new Guid("{4329112a-6b65-48d9-9da8-abf1f8f36327}");
            public static Guid WallFoundation = new Guid("{d7dd0293-dd65-4229-a64c-8b528d4e226f}");
            public static Guid Window = new Guid("{2b02b353-2ca5-4566-88bb-917ea8460174}");
            public static Guid WiringAccessory = new Guid("{b00d5c25-92a8-4409-a3b7-7c37ed792c06}");
        }
    }
}
