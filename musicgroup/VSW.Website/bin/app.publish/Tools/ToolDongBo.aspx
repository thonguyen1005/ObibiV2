<%@ Page Language="C#" AutoEventWireup="true" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        var list = ModUrlSeoService.Instance.CreateQuery()
                    .ToList();

        for (int i = 0; list != null && i < list.Count; i++)
        {
            string url = list[i].Url;
            var check = ModUrlSeoDashboardService.Instance.GetByUrlCache(url);
            if (check == null)
            {
                check = new ModUrlSeoDashboardEntity();
                check.ID = 0;
                check.Url = url;
                check.UrlRedirect = list[i].UrlRedirect;
                check.CountSearch = list[i].CountSearch;
            }
            else
            {
                check.CountSearch += list[i].CountSearch;
            }
            ModUrlSeoDashboardService.Instance.Save(check);
        }
        ModUrlSeoService.Instance.Delete(list);
        
        Response.Write("OK");
    }

</script>
