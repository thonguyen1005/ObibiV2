using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModTagEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public string Code { get; set; }
        [DataInfo]
        public string Link { get; set; }
        [DataInfo]
        public string Title { get; set; }
        [DataInfo]
        public string Keywords { get; set; }
        [DataInfo]
        public string Description { get; set; }
        [DataInfo]
        public int View { get; set; }

        #endregion
        public void UpView()
        {
            View++;
            ModTagService.Instance.Save(this, o => o.View);
        }
    }

    public class ModTagService : ServiceBase<ModTagEntity>
    {
        #region Autogen by VSW

        private ModTagService()
            : base("[Mod_Tag]")
        {

        }

        private static ModTagService _Instance = null;
        public static ModTagService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModTagService();

                return _Instance;
            }
        }

        #endregion

        public ModTagEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public ModTagEntity GetByCode(string code)
        {
            return base.CreateQuery()
               .Where(o => o.Code == code)
               .ToSingle();
        }

        public void UpdateProductTag(int product_id, string tag)
        {
            ModProductTagService.Instance.Delete(o => o.ProductID == product_id);

            if (tag.Trim() == string.Empty) return;

            string[] ArrTag = tag.Split(',');
            for (int i = 0; i < ArrTag.Length; i++)
            {
                string name = ArrTag[i].Trim();
                string code = VSW.Lib.Global.Data.GetCode(name);

                if (code != string.Empty)
                {
                    ModTagEntity _Tag = GetByCode(code);

                    if (_Tag == null)
                    {
                        _Tag = new ModTagEntity()
                        {
                            Name = name,
                            Code = code
                        };

                        Save(_Tag);
                    }

                    ModProductTagService.Instance.Save(new ModProductTagEntity()
                    {
                        ProductID = product_id,
                        TagID = _Tag.ID
                    });
                }
            }
        }
        
        public void UpdateNewsTag(int news_id, string tag)
        {
            ModNewsTagService.Instance.Delete(o => o.NewsID == news_id);

            if (tag.Trim() == string.Empty) return;

            string[] ArrTag = tag.Split(',');
            for (int i = 0; i < ArrTag.Length; i++)
            {
                string name = ArrTag[i].Trim();
                string code = VSW.Lib.Global.Data.GetCode(name);

                if (code != string.Empty)
                {
                    ModTagEntity _Tag = GetByCode(code);

                    if (_Tag == null)
                    {
                        _Tag = new ModTagEntity()
                        {
                            Name = name,
                            Code = code
                        };

                        Save(_Tag);
                    }

                    ModNewsTagService.Instance.Save(new ModNewsTagEntity()
                    {
                        NewsID = news_id,
                        TagID = _Tag.ID
                    });
                }
            }
        }
  
    }
}