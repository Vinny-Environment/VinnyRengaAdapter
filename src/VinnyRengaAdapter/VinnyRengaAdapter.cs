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
using static VinnyRengaAdapter.VinnyRengaUtils;
using System.Diagnostics;


namespace VinnyRengaAdapter
{
    class RengaLevelInfo : IComparable
    {
        public string Name { get; set; }
        public double Elevation { get; set; }

        public int RengaModelObjectId { get; set; }

        public const double nonLevelElevation = 1000000000000;

        public RengaLevelInfo()
        {
            Elevation = nonLevelElevation;
        }

        public int CompareTo(object obj)
        {
            return Elevation.CompareTo(((RengaLevelInfo)obj).Elevation);
        }
    }
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
            var timer = new Stopwatch();
            timer.Start();
            VinnyLibConverterUI.VLC_UI_MainWindow vinnyWindow = new VinnyLibConverterUI.VLC_UI_MainWindow(false);
            VinnyLibConverterCommon.ImportExportParameters parameters = new ImportExportParameters();

#if DEBUG
            parameters = VinnyLibConverterCommon.ImportExportParameters.LoadFromFile(@"E:\Temp\Vinny\rengaTestParams1.XML");
#else
            if (vinnyWindow.ShowDialog() == true) parameters = vinnyWindow.VinnyParametets;
#endif
            ExportTo(CreateData(), parameters);

            timer.Stop();
            string time = $"Обработка завершена!\nЗатраченное время {timer.Elapsed.TotalSeconds} с.";

            System.Windows.MessageBox.Show(time);
        }

        private const string RengaParamUniqueId = "RengaUniqueId";
        private const string RengaParamCategory_Properties = "Renga Properties";
        private VinnyLibDataStructureParameterValue SetUniqueIdParam(string id, string category = VinnyLibDataStructureParametersManager.CategoryDefaultName)
        {
            return mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs(RengaParamUniqueId, id, category);
        }

        private void ProcessRengaProperties(Renga.IPropertyContainer rengaProperties, VinnyLibDataStructureObjectWithParametersBase vinnyObjectDef)
        {
            Renga.IGuidCollection rengaPropsIds = rengaProperties.GetIds();
            for (int rengaIdCounter = 0; rengaIdCounter < rengaPropsIds.Count; rengaIdCounter++)
            {
                Guid rengaPropId = rengaPropsIds.Get(rengaIdCounter);
                Renga.IProperty rengaPropDef = rengaProperties.Get(rengaPropId);

                object propValue;
                VinnyLibDataStructureParameterDefinitionType propType;
                VinnyRengaUtils.GetPropertyValueAndTupe(rengaPropDef, out propValue, out propType);
                var vinnyPropValue = mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs(rengaPropDef.Name, propValue, RengaParamCategory_Properties, propType);
                vinnyObjectDef.Parameters.Add(vinnyPropValue);
            }
        }

        private void ProcessRengaModelObjectProps(Renga.IModelObject rengaObject, VinnyLibDataStructureObjectWithParametersBase vinnyObjectDef)
        {
            ProcessRengaProperties(rengaObject.GetProperties(), vinnyObjectDef);
            //Quatities
            Renga.IQuantityContainer rengaQuantitiesCollection = rengaObject.GetQuantities();

            foreach (var rengaQuantitiInfo in RengaQuantityIds.QuantityIdentifiers_Objects())
            {
                if (rengaQuantitiesCollection.Contains(rengaQuantitiInfo.Value))
                {
                    Renga.IQuantity rengaQuantity = rengaQuantitiesCollection.Get(rengaQuantitiInfo.Value);
                    object propValue;
                    VinnyLibDataStructureParameterDefinitionType propType;
                    VinnyRengaUtils.GetQuatityValueAndType(rengaQuantity, out propValue, out propType);
                    vinnyObjectDef.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs(rengaQuantitiInfo.Key, propValue, "Renga Quantities", propType));
                }
            }

            //Parameters
            Renga.IParameterContainer rengaParametersCollection = rengaObject.GetParameters();
            Renga.IGuidCollection rengaParametersIds = rengaParametersCollection.GetIds();
            for (int rengaIdCounter = 0; rengaIdCounter < rengaParametersIds.Count; rengaIdCounter++)
            {
                Guid rengaParameterId = rengaParametersIds.Get(rengaIdCounter);
                Renga.IParameter rengaParameterDef = rengaParametersCollection.Get(rengaParameterId);

                object propValue;
                VinnyLibDataStructureParameterDefinitionType propType;
                VinnyRengaUtils.GetParameterValueAndType(rengaParameterDef, out propValue, out propType);
                vinnyObjectDef.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs(rengaParameterDef.Definition.Text, propValue, "Renga Parameters", propType));
            }
        }

        private void SetRengaProjectMetadata(Renga.IProject rengaProject)
        {
            //Метаданные проекта
            Renga.IProjectInfo rengaProjectInfo = rengaProject.ProjectInfo;
            string rengaProjectInfoCat = "Renga Project Info";

            mVinnyModelDef.Header.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaProjectCode", rengaProjectInfo.Code, rengaProjectInfoCat));
            mVinnyModelDef.Header.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaProjectName", rengaProjectInfo.Name, rengaProjectInfoCat));
            mVinnyModelDef.Header.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaProjectStage", rengaProjectInfo.Stage, rengaProjectInfoCat));
            mVinnyModelDef.Header.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaProjectDescription", rengaProjectInfo.Description, rengaProjectInfoCat));
            mVinnyModelDef.Header.Parameters.Add(SetUniqueIdParam(rengaProjectInfo.UniqueIdS));

            ProcessRengaProperties(rengaProjectInfo.GetProperties(), mVinnyModelDef.Header);
        }

        private int SetRengaSite(Renga.ILandPlotInfo rengaProjectLandPlotInfo)
        {
            VinnyLibDataStructureObject vinnySiteObject = mVinnyModelDef.ObjectsManager.GetObjectById(mVinnyModelDef.ObjectsManager.CreateObject());
            vinnySiteObject.Name = rengaProjectLandPlotInfo.Name;

            string rengaProjectLandPlotInfoCat = "Renga Land Info";
            string rengaProjectLandAddressCat = "Renga Land Address";

            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandName", rengaProjectLandPlotInfo.Name, rengaProjectLandPlotInfoCat));
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandDescription", rengaProjectLandPlotInfo.Description, rengaProjectLandPlotInfoCat));
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandNumber", rengaProjectLandPlotInfo.Number, rengaProjectLandPlotInfoCat));
            vinnySiteObject.Parameters.Add(SetUniqueIdParam(rengaProjectLandPlotInfo.UniqueIdS));
            ProcessRengaProperties(rengaProjectLandPlotInfo.GetProperties(), vinnySiteObject);
            // адрес
            Renga.IPostalAddress rengaProjectLandPlotInfo_Address = rengaProjectLandPlotInfo.GetAddress();
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandAddressRegion", rengaProjectLandPlotInfo_Address.Region, rengaProjectLandAddressCat));
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandAddressTown", rengaProjectLandPlotInfo_Address.Town, rengaProjectLandAddressCat));
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandAddressAddressee", rengaProjectLandPlotInfo_Address.Addressee, rengaProjectLandAddressCat));
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandAddressPostalBox", rengaProjectLandPlotInfo_Address.PostalBox, rengaProjectLandAddressCat));
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandAddressCountry", rengaProjectLandPlotInfo_Address.Country, rengaProjectLandAddressCat));
            string addressLinesData = "";
            if (rengaProjectLandPlotInfo_Address.AddressLines.Cast<string>().Any()) addressLinesData = string.Join(";", rengaProjectLandPlotInfo_Address.AddressLines.Cast<string>().ToArray());
            vinnySiteObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLandAddressAddressLines", addressLinesData, rengaProjectLandAddressCat));

            mVinnyModelDef.ObjectsManager.SetObject(vinnySiteObject.Id, vinnySiteObject);

            return vinnySiteObject.Id;
        }

        private int SetRengaBuilding(Renga.IBuildingInfo rengaProjectBuildingInfo, int vinnyRengaSiteId)
        {
            VinnyLibDataStructureObject vinnyBuildingObject = mVinnyModelDef.ObjectsManager.GetObjectById(mVinnyModelDef.ObjectsManager.CreateObject());
            vinnyBuildingObject.Name = rengaProjectBuildingInfo.Name;
            vinnyBuildingObject.ParentId = vinnyRengaSiteId;


            string rengaProjectBuildingInfoCat = "Renga Building Info";
            vinnyBuildingObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaBuildingInfoName", rengaProjectBuildingInfo.Name, rengaProjectBuildingInfoCat));
            vinnyBuildingObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaBuildingInfoDescription", rengaProjectBuildingInfo.Description, rengaProjectBuildingInfoCat));
            vinnyBuildingObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaBuildingInfoNumber", rengaProjectBuildingInfo.Number, rengaProjectBuildingInfoCat));
            vinnyBuildingObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaBuildingInfoId", rengaProjectBuildingInfo.Id, rengaProjectBuildingInfoCat));
            vinnyBuildingObject.Parameters.Add(SetUniqueIdParam(rengaProjectBuildingInfo.UniqueIdS));
            ProcessRengaProperties(rengaProjectBuildingInfo.GetProperties(), vinnyBuildingObject);

            mVinnyModelDef.ObjectsManager.SetObject(vinnyBuildingObject.Id, vinnyBuildingObject);

            return vinnyBuildingObject.Id;
        }
    

        public VinnyLibDataStructureModel CreateData()
        {
            mVinnyModelDef = new VinnyLibDataStructureModel();
            Renga.Application rengaApp = new Renga.Application();
            Renga.IProject rengaProject = rengaApp.Project;
            Renga.IDataExporter rengaDataExporter = rengaProject.DataExporter;
            Renga.IExportedObject3DCollection rengaExportedObject3DCollection = rengaDataExporter.GetObjects3D();

            SetRengaProjectMetadata(rengaProject);
            //Участок -> Здание -> Всё остальное

            //Участок
            int vinnyRengaSiteId = SetRengaSite(rengaProject.LandPlotInfo);
            //Здание
            int vinnyRengaBuildingId = SetRengaBuilding(rengaProject.BuildingInfo, vinnyRengaSiteId);

            Renga.IMaterialManager rebgaNaterialManager = rengaProject.MaterialManager;

            //Видимость
            Renga.IModelView view3d = rengaApp.ActiveView as Renga.IModelView;
            if (view3d == null)
            {
                throw new Exception("VinnyRenga Текущий активный вид отличен от 3D");
            }

            int[] VisibleObjects = view3d.GetVisibleObjects().Cast<int>().ToArray();

            //1. Сопоставление ModelObject.Id с IExportedObject3D
            Dictionary<int, Renga.IExportedObject3D> rengaModelObjectId2Geometry = new Dictionary<int, Renga.IExportedObject3D>();

            for (int rengaObjectGeometryCounter = 0; rengaObjectGeometryCounter < rengaExportedObject3DCollection.Count; rengaObjectGeometryCounter++)
            {
                Renga.IExportedObject3D rengaObjectGeometry = rengaExportedObject3DCollection.Get(rengaObjectGeometryCounter);
                rengaModelObjectId2Geometry[rengaObjectGeometry.ModelObjectId] = rengaObjectGeometry;
            }

            //2. Итеративный перебор объектов модели
            // Будем выгружать по уровням, отсортированнам по высоте. Сперва тестовый прогон по всем объектам с целью определить привязку к уровню
            Dictionary<int, RengaLevelInfo> levelExtendedInfo = new Dictionary<int, RengaLevelInfo>();
            Dictionary<RengaLevelInfo, List<Renga.IModelObject>> level2Objects = new Dictionary<RengaLevelInfo, List<Renga.IModelObject>>();
            List<Renga.IModelObject> nonLevelsObjects = new List<Renga.IModelObject>();

            Renga.IModelObjectCollection rengaModelObjectCollection = rengaProject.Model.GetObjects();
            int[] rengaModelObjectIds = rengaModelObjectCollection.GetIds().Cast<int>().ToArray();
            for (int rengaObjectCounter = 0; rengaObjectCounter < rengaModelObjectIds.Length; rengaObjectCounter++)
            {
                Renga.IModelObject rengaObject = rengaModelObjectCollection.GetById(rengaModelObjectIds[rengaObjectCounter]);
                if (rengaObject.ObjectType == RengaObjectTypes.Level) continue;

                Renga.ILevelObject rengaObjectOnLevel = rengaObject as Renga.ILevelObject;
                if (rengaObjectOnLevel != null)
                {
                    if (!levelExtendedInfo.ContainsKey(rengaObjectOnLevel.LevelId))
                    {
                        //Вообще, никогда не должно быть null
                        Renga.ILevel rengaLevel = rengaModelObjectCollection.GetById(rengaObjectOnLevel.LevelId) as Renga.ILevel;
                        RengaLevelInfo rengaLevelInfo = new RengaLevelInfo() { Name = rengaLevel.LevelName, Elevation = rengaLevel.Elevation, RengaModelObjectId = rengaObjectOnLevel.LevelId };
                        

                        levelExtendedInfo.Add(rengaObjectOnLevel.LevelId, rengaLevelInfo);
                        level2Objects.Add(levelExtendedInfo[rengaObjectOnLevel.LevelId], new List<Renga.IModelObject>());
                    }

                    level2Objects[levelExtendedInfo[rengaObjectOnLevel.LevelId]].Add(rengaObject);
                }
                else nonLevelsObjects.Add(rengaObject);
            }

            level2Objects = level2Objects.OrderBy(a=>a.Key).ToDictionary(t=> t.Key, t => t.Value);
            level2Objects.Add(new RengaLevelInfo() { Name = "Прочие объекты" }, nonLevelsObjects);

            foreach (var levelObjectsCollection in level2Objects)
            {
                
                RengaLevelInfo levelInfo = levelObjectsCollection.Key;
                if (!VisibleObjects.Contains(levelInfo.RengaModelObjectId)) continue;

                VinnyLibDataStructureObject vinnyLevelObject = mVinnyModelDef.ObjectsManager.GetObjectById(mVinnyModelDef.ObjectsManager.CreateObject());
                vinnyLevelObject.Name = levelInfo.Name;
                vinnyLevelObject.ParentId = vinnyRengaBuildingId;
                ProcessRengaProperties(rengaModelObjectCollection.GetById(levelInfo.RengaModelObjectId).GetProperties(), vinnyLevelObject);

                if (levelInfo.Elevation != RengaLevelInfo.nonLevelElevation) vinnyLevelObject.Parameters.Add(mVinnyModelDef.ParametersManager.CreateParameterValueWithDefs("RengaLevelElevation", levelObjectsCollection.Key.Elevation, RengaParamCategory_Properties, VinnyLibDataStructureParameterDefinitionType.ParamReal));
                mVinnyModelDef.ObjectsManager.SetObject(vinnyLevelObject.Id, vinnyLevelObject);

                foreach (Renga.IModelObject rengaObject in levelObjectsCollection.Value)
                {
                    if (!VisibleObjects.Contains(rengaObject.Id)) continue;

                    VinnyLibDataStructureObject vinnyObject = mVinnyModelDef.ObjectsManager.GetObjectById(mVinnyModelDef.ObjectsManager.CreateObject());
                    vinnyObject.Name = rengaObject.Name;
                    vinnyObject.UniqueId = rengaObject.UniqueIdS;
                    ProcessRengaModelObjectProps(rengaObject, vinnyObject);
                    vinnyObject.ParentId = vinnyLevelObject.Id;

                    //Геометрия
                    if (rengaModelObjectId2Geometry.ContainsKey(rengaObject.Id))
                    {
                        Renga.IExportedObject3D rengaObjectGeometry = rengaModelObjectId2Geometry[rengaObject.Id];

                        for (int rengaMeshCounter = 0; rengaMeshCounter < rengaObjectGeometry.MeshCount; rengaMeshCounter++)
                        {
                            Renga.IMesh mesh = rengaObjectGeometry.GetMesh(rengaMeshCounter);

                            int gridPointsCount = 0;
                            int gridFacesCount = 0;
                            for (int rengaGridCounter = 0; rengaGridCounter < mesh.GridCount; rengaGridCounter++)
                            {
                                Renga.IGrid grid = mesh.GetGrid(rengaGridCounter);
                                int gridType = grid.GridType;
                                bool isFind = false;

                                int materialIndex = GetMaterial(rengaObject, out isFind, rengaMeshCounter, gridType);

                                VinnyLibDataStructureGeometryMesh vinnyGeometryMesh = VinnyLibDataStructureGeometryMesh.asType(mVinnyModelDef.GeometrtyManager.GetMeshGeometryById(mVinnyModelDef.GeometrtyManager.CreateGeometry(VinnyLibDataStructureGeometryType.Mesh)));
                                vinnyGeometryMesh.MaterialId = materialIndex;

                                for (int rengaVertexCounter = 0; rengaVertexCounter < grid.VertexCount; rengaVertexCounter++)
                                {
                                    Renga.FloatPoint3D p = grid.GetVertex(rengaVertexCounter);
                                    vinnyGeometryMesh.AddVertex(p.X, p.Y, p.Z);

                                }
                                for (int rengaFaceCounter = 0; rengaFaceCounter < grid.TriangleCount; rengaFaceCounter++)
                                {
                                    Renga.Triangle tr = grid.GetTriangle(rengaFaceCounter);
                                    vinnyGeometryMesh.AddFace((int)tr.V0 + gridPointsCount, (int)tr.V1 + gridPointsCount, (int)tr.V2 + gridPointsCount);
                                    vinnyGeometryMesh.AssignMaterialToFace(rengaFaceCounter + gridFacesCount, materialIndex);
                                }
                                //gridPointsCount += grid.VertexCount;
                                //gridFacesCount += grid.TriangleCount;

                                mVinnyModelDef.GeometrtyManager.SetMeshGeometry(vinnyGeometryMesh.Id, vinnyGeometryMesh);
                                int vinnyGeometryMeshPIid = mVinnyModelDef.GeometrtyManager.CreateGeometryPlacementInfo(vinnyGeometryMesh.Id);

                                vinnyObject.GeometryPlacementInfoIds.Add(vinnyGeometryMeshPIid);
                            }

                            
                        }
                    }
                    mVinnyModelDef.ObjectsManager.SetObject(vinnyObject.Id, vinnyObject);
                }
            }

            int GetMaterial(Renga.IModelObject rengaObject, out bool isFind, int gridPos = -1, int gridType = -1)
            {
                isFind = false;
                if (rengaObject.ObjectType == VinnyRengaUtils.RengaObjectTypes.Window || rengaObject.ObjectType == VinnyRengaUtils.RengaObjectTypes.Door)
                {
                    int[] colorDef = new int[4] { 0, 0, 0, 0 };
                    if (rengaObject.ObjectType == VinnyRengaUtils.RengaObjectTypes.Window)
                    {
                        switch (gridType)
                        {
                            case (int)Renga.GridTypes.Window.Frame:
                                colorDef = new int[] { 186, 152, 70 };
                                break;
                            case (int)Renga.GridTypes.Window.Glass:
                                colorDef = new int[] { 194, 246, 237, 153 };
                                break;
                            case (int)Renga.GridTypes.Window.Sill:
                                colorDef = new int[] { 172, 172, 172 };
                                break;
                            case (int)Renga.GridTypes.Window.OutwardSill:
                                colorDef = new int[] { 172, 172, 172 };
                                break;
                        }
                    }
                    else if (rengaObject.ObjectType == VinnyRengaUtils.RengaObjectTypes.Door)
                    {
                        switch (gridType)
                        {
                            case (int)Renga.GridTypes.Door.Frame:
                                colorDef = new int[] { 153, 153, 0 };
                                break;
                            case (int)Renga.GridTypes.Door.Glass:
                                colorDef = new int[] { 194, 246, 237, 153 };
                                break;
                            case (int)Renga.GridTypes.Door.Solid:
                                colorDef = new int[] { 153, 153, 0 };
                                break;
                            case (int)Renga.GridTypes.Door.DoorLining:
                                colorDef = new int[] { 102, 0, 0 };
                                break;
                            case (int)Renga.GridTypes.Door.Threshold:
                                colorDef = new int[] { 102, 0, 0 };
                                break;
                        }
                    }

                    isFind = true;
                    return mVinnyModelDef.MaterialsManager.CreateMaterial(colorDef);
                }
                
                Renga.IObjectWithLayeredMaterial objectLayeredMaterial = rengaObject as Renga.IObjectWithLayeredMaterial;
                if (objectLayeredMaterial != null && objectLayeredMaterial.HasLayeredMaterial())
                {
                    Renga.ILayerCollection rengaLayerCollection = objectLayeredMaterial.GetLayers();
                    if (gridPos != -1 && gridPos < rengaLayerCollection.Count)
                    {
                        Renga.ILayer layer = rengaLayerCollection.Get(gridPos);
                        Renga.IMaterial rengaMaterialDef = rengaApp.Project.MaterialManager.GetMaterial(layer.MaterialId);
                        isFind = true;

                        return mVinnyModelDef.MaterialsManager.CreateMaterial(new int[] { rengaMaterialDef.Color.Red, rengaMaterialDef.Color.Green, rengaMaterialDef.Color.Blue, rengaMaterialDef.Color.Alpha }, rengaMaterialDef.Name);
                    }
                }

                Renga.IObjectWithMaterial objectMat = rengaObject as Renga.IObjectWithMaterial;
                if (objectMat != null && objectMat.HasMaterial())
                {
                    Renga.IMaterial rengaMaterialDef = rengaApp.Project.MaterialManager.GetMaterial(objectMat.MaterialId);

                    isFind = true;
                    return mVinnyModelDef.MaterialsManager.CreateMaterial(new int[] { rengaMaterialDef.Color.Red, rengaMaterialDef.Color.Green, rengaMaterialDef.Color .Blue, rengaMaterialDef.Color .Alpha}, rengaMaterialDef.Name);
                }

                if (rengaObject.ObjectType == VinnyRengaUtils.RengaObjectTypes.Route)
                {
                    Renga.IRouteParams rengaRouteParams = rengaObject.GetInterfaceByName("IRouteParams") as Renga.IRouteParams;
                    if (rengaRouteParams != null)
                    {
                        Renga.ISystemStyleManager rengaSystemStyleManager = rengaApp.Project.SystemStyleManager;
                        if (rengaSystemStyleManager.Contains(rengaRouteParams.SystemStyleId))
                        {
                            Renga.ISystemStyle rengaSystemStyle = rengaSystemStyleManager.GetSystemStyle(rengaRouteParams.SystemStyleId);
                            isFind = true;
                            return mVinnyModelDef.MaterialsManager.CreateMaterial(new int[] { rengaSystemStyle.Color.Red, rengaSystemStyle.Color.Green, rengaSystemStyle.Color.Blue, rengaSystemStyle.Color.Alpha }, rengaSystemStyle.Name);
                        }
                    }
                }

                

                return 0; //defaule color
            }

            return mVinnyModelDef;
        }

        public void ExportTo(VinnyLibDataStructureModel vinnyData, ImportExportParameters outputParameters)
        {
            mConverter.ExportModel(vinnyData, outputParameters);
        }

        private VinnyLibDataStructureModel mVinnyModelDef;
        private static VinnyRengaAdapter mInstance;
        private static VinnyLibConverterKernel.VinnyLibConverter mConverter;
    }
}
