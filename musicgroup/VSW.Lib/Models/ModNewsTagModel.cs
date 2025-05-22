using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModNewsTagEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public int TagID { get; set; }
        [DataInfo]
        public int NewsID { get; set; }

        #endregion      
  
        private ModTagEntity _oTag = null;
        public ModTagEntity getTag()
        {
            if (_oTag == null && TagID > 0)
                _oTag = ModTagService.Instance.GetByID(TagID);

            if (_oTag == null)
                _oTag = new ModTagEntity();

            return _oTag;
        }      
  
        private ModNewsEntity _oNews = null;
        public ModNewsEntity getNews()
        {
            if (_oNews == null && NewsID > 0)
                _oNews = ModNewsService.Instance.GetByID(NewsID);

            if (_oNews == null)
                _oNews = new ModNewsEntity();

            return _oNews;
        }

    }

    public class ModNewsTagService : ServiceBase<ModNewsTagEntity>
    {

        #region Autogen by VSW

        private ModNewsTagService()
            : base("[Mod_NewsTag]")
        {

        }

        private static ModNewsTagService _Instance = null;
        public static ModNewsTagService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModNewsTagService();

                return _Instance;
            }
        }

        #endregion

        public ModNewsTagEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

    }
}