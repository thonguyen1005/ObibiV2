using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductFileEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public string ProductSizeID { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public bool Default { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public bool IsAvatar { get; set; }

        #endregion Autogen by VSW

        private ModProductEntity _oProduct;
        public ModProductEntity GetProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID_Cache(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }
    }

    public class ModProductFileService : ServiceBase<ModProductFileEntity>
    {
        #region Autogen by VSW

        public ModProductFileService() : base("[Mod_ProductFile]")
        {
        }

        private static ModProductFileService _instance;
        public static ModProductFileService Instance => _instance ?? (_instance = new ModProductFileService());

        #endregion Autogen by VSW

        public ModProductFileEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModProductFileEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int productID, string file)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.File == file)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

        public List<ModProductFileEntity> GetAll_Cache(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList_Cache();
        }

        public void InsertOrUpdate(int productID, string[] arrFile)
        {
            if (arrFile == null || arrFile.Length == 0)
            {
                Delete(o => o.ProductID == productID);
                return;
            }

            var listInDb = CreateQuery()
                                .Where(o => o.ProductID == productID)
                                .ToList_Cache();

            for (var i = 0; listInDb != null && i < listInDb.Count; i++)
            {
                if (System.Array.IndexOf(arrFile, listInDb[i].File) < 0)
                    Delete(listInDb[i]);
            }

            foreach (var file in arrFile)
            {
                if (string.IsNullOrEmpty(file)) continue;

                var file1 = file;
                var item = CreateQuery().Where(o => o.ProductID == productID && o.File == file1).ToSingle_Cache();
                if (item != null) continue;

                item = new ModProductFileEntity()
                {
                    ProductID = productID,
                    File = file,
                    Default = false
                };

                Save(item);
            }
        }
    }
}