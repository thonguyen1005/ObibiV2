<%@ Control Language="C#" AutoEventWireup="true" %>
<script runat="server">

    private string _cphName = string.Empty;
    public string CphName
    {
        get { return _cphName; }
        set { _cphName = value; }
    }
</script>

<div class="border-template">
    <div class="title-template"><%= CphName %></div>
    