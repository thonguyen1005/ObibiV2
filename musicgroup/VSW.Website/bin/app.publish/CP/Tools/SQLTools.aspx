<%@ Page Language="C#" AutoEventWireup="true" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CPLogin.IsLogin() && CPLogin.CurrentUser.IsAdministrator) return;

        Response.Redirect("Login.aspx?ReturnPath=" + Server.UrlEncode(Request.RawUrl));
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        if (txtSQL.Text == string.Empty) return;

        var ds = VSW.Core.Data.SQLCmd.ExecuteDataSet(VSW.Core.Data.ConnectionString.Default, txtSQL.Text);
        if (ds == null || ds.Tables.Count == 0) return;

        gvSQL.DataSource = ds;
        gvSQL.DataBind();
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SQL</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="text-align: center;" >
                <br/>
                <asp:TextBox ID="txtSQL" TextMode="MultiLine" runat="server" Height="200px" Width="574px"></asp:TextBox>
                <br/>
                <asp:Button ID="btnRun" runat="server" OnClick="btnRun_Click" Text="Run" Width="111px" />
                <br/>
                <br/>
                <asp:GridView ID="gvSQL" runat="server"></asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>