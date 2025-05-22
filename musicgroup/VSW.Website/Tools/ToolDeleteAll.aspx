<%@ Page Language="C#" AutoEventWireup="true" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        var list = ModUrlSeoDashboardService.Instance.CreateQuery()
                    .Where(o => o.CountSearch <= 30)
                    .ToList();

        if (list != null)
        {
            ModUrlSeoDashboardService.Instance.Delete(list);
        }
        Response.Write("OK");
    }

</script>
