<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var listItem = ViewBag.Data as List<SysPageEntity>;
    var page = ViewBag.Page as SysPageEntity;
    if (page == null || listItem == null) return;
    if (listItem.Count == 0) return;
%>

<div class="dok-category d-lg-block d-none">
    <a class="dok-header-item" href="javascript:void(0)">
        <i class="far fa-bars-staggered"></i>
        <span>Danh mục</span>
    </a>
</div>

<div id="menu-main" class="menu-container" style="display: none;">
    <div class="menu-wrapper space-bread-crumb" style="top: 70px;">
        <div class="menu-tree">
            <%for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var listSubChildItem = SysPageService.Instance.GetByParent_Cache(listItem[i].ID);
                    var listPropertysLv1 = WebPropertyConfigService.Instance.CreateQuery().Where(o => o.MenuID == listItem[i].MenuID && o.IsShowMenu == true).ToList_Cache();

            %>
            <div class="label-menu-tree" target="_self">
                <div class="label-menu-item">
                    <div class="right-content">
                        <%if (!string.IsNullOrEmpty(listItem[i].Icon))
                            { %>
                        <%if (listItem[i].Icon.Contains("/"))
                            { %>
                        <img width="25" height="25" class="lazyload" src="<%=Utils.GetWebPFile(listItem[i].Icon, 4, 25, 25) %>" alt="<%=listItem[i].Name %>" />
                        <%}
                            else
                            { %>
                        <i class="ic-cate <%=listItem[i].Icon %>"></i>
                        <%} %>
                        <%} %>

                        <a href="<%=ViewPage.GetPageURL(listItem[i]) %>" title="<%=listItem[i].Name %>" class="multiple-link"><span><%=listItem[i].Name %></span></a>
                    </div>
                    <div class="menu-ic-right">
                        <i class="fas fa-chevron-right"></i>
                    </div>
                </div>
                <%if ((listSubChildItem != null && listSubChildItem.Count > 0) || listPropertysLv1 != null)
                    { %>
                <div class="menu-tree-child">
                    <div class="menu-tree-child-wrapper columns box">
                        <%if (listPropertysLv1 != null)
                            {
                                var listBrand = listPropertysLv1.Where(o => o.BrandID > 0).ToList();
                        %>
                        <div class="menu-child-item column group is-one-fifth">
                            <strong class="group-title">Thương hiệu <%=listItem[i].Name %></strong>
                            <%for (int g = 0; listBrand != null && g < listBrand.Count; g++)
                                {
                                    var brand = WebMenuService.Instance.GetByID_Cache(listBrand[g].BrandID);
                                    if (brand == null) continue;
                            %>
                            <div class="menu-child-item column menu-item">
                                <a href="<%=ViewPage.GetURL(listItem[i].Code+"-"+brand.Code) %>#b=<%=brand.ID %>" title="<%=brand.Name %>" class="label-wrapper">
                                    <div class="label-item">
                                        <span><%=brand.Name %></span>
                                    </div>
                                </a>
                            </div>
                            <%} %>
                        </div>
                        <%} %>

                        <%if (listSubChildItem != null && listSubChildItem.Count > 0)
                            {
                        %>
                        <div class="menu-child-item column group is-one-fifth">
                            <strong class="group-title">Loại <%=listItem[i].Name %></strong>
                            <%for (int k = 0; listSubChildItem != null && k < listSubChildItem.Count; k++)
                                {
                            %>
                            <div class="menu-child-item column menu-item">
                                <a href="<%=ViewPage.GetPageURL(listSubChildItem[k]) %>" title="<%=listSubChildItem[k].Name %>" class="label-wrapper">
                                    <div class="label-item">
                                        <span><%=listSubChildItem[k].Name %></span>
                                    </div>
                                </a>
                            </div>
                            <%} %>
                        </div>
                        <%} %>

                        <%if (listPropertysLv1 != null)
                            {
                                var listProperty = listPropertysLv1.Where(o => o.PropertyID > 0).ToList();
                                var lstGroupProperty = new List<int>();
                                if (listProperty != null)
                                {
                                    lstGroupProperty = listProperty.GroupBy(o => o.PropertyParentID).Select(o => o.Key).ToList();
                                }
                        %>
                        <%for (int g = 0; lstGroupProperty != null && g < lstGroupProperty.Count; g++)
                            {
                                if (lstGroupProperty[g] < 1) continue;
                                var p = WebPropertyService.Instance.GetByID_Cache(lstGroupProperty[g]);
                                if (p == null) continue;
                                var listPropertyByParent = listProperty.Where(o => o.PropertyParentID == lstGroupProperty[g]).ToList();
                        %>
                        <div class="menu-child-item column group is-one-fifth">
                            <strong class="group-title"><%=p.Name %></strong>
                            <%for (int h = 0; listPropertyByParent != null && h < listPropertyByParent.Count; h++)
                                {
                                    var property = WebPropertyService.Instance.GetByID_Cache(listPropertyByParent[h].PropertyID);
                                    if (property == null) continue;
                            %>
                            <div class="menu-child-item column menu-item">
                                <a href="<%=ViewPage.GetURL(listItem[i].Code+"-"+property.Code) %>#c=<%=property.ID %>" title="<%=property.Name %>" class="label-wrapper">
                                    <div class="label-item">
                                        <span><%=property.Name %></span>
                                    </div>
                                </a>
                            </div>
                            <%}%>
                        </div>
                        <% } %>
                        <%} %>
                    </div>
                </div>
                <%} %>
            </div>
            <%} %>
        </div>
    </div>
</div>
