<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<% 
    var item = ViewBag.Data as ModNewsEntity;
    var listItem = ViewBag.Other as List<ModNewsEntity>;
    var ArrTag = item.Tags.Split(',');
    string content = Utils.GetHtmlForSeo(item.Content);
    string htmlProduct = "";
    var lstProduct = item.GetProduct();
    if (lstProduct != null && lstProduct.Count > 0)
    {
        htmlProduct += @"<div class=""dok-products-post dok-slider-box swiper mb-3"">";
        htmlProduct += @"<div class=""swiper-wrapper"">";
        for (int i = 0; lstProduct != null && i < lstProduct.Count; i++)
        {
            string url = ViewPage.GetURL(lstProduct[i].MenuID, lstProduct[i].Code);
            var countvote = ModVoteService.Instance.CreateQuery().Where(o => o.ProductID == lstProduct[i].ID && o.Type == "Product" && o.Activity == true).Count().ToValue_Cache().ToInt();
            var countBuyItem = VSW.Core.Global.DAO<GetSumModel>.Populate(ModOrderDetailService.Instance.ExecuteReader("select sum([Quantity]) from " + ModOrderDetailService.Instance.TableName + " where [ProductID] = " + lstProduct[i].ID));
            var hasPropertyItem = ModProductClassifyService.Instance.CreateQuery().Where(o => o.ProductID == lstProduct[i].ID).Count().ToValue_Cache().ToBool();

            htmlProduct += @"<div class=""swiper-slide"">";
            htmlProduct += @"<div class=""product"">
                        <div class=""product-info-container"">
                            <div class=""product-info"">
                                <a href=""" + url + @""" title=""" + lstProduct[i].Name + @""" class=""product__link button__link"">
                                    <div class=""product__image"">
                                        <img src=""" + Utils.GetWebPFile(lstProduct[i].File, 4, 358, 358) + @"""
                                            width=""358"" height=""358""
                                            alt=""" + lstProduct[i].Name + @"""
                                            class=""product__img"">
                                    </div>
                                    <div class=""product__name"">
                                        <h3 class=""mb-0"">" + lstProduct[i].Name + @"</h3>
                                    </div>";
            if (countBuyItem != null && countBuyItem.Count > 0)
            {
                htmlProduct += @"<div class=""block-smem-price"">" + countBuyItem[0].Total + @" sản phẩm đã bán</div>";
            }
            htmlProduct += @"<div class=""bottom-div"">
                                        <div class=""product__box-rating"">
                                            <div>";
            for (int z = 0; z < lstProduct[i].Star; z++)
            {
                htmlProduct += @"<div class=""ic-star is-active"">
                                                    <i class=""fas fa-star-sharp""></i>
                                                </div>";
            }
            for (int z = 0; z < 5 - lstProduct[i].Star; z++)
            {
                htmlProduct += @"<div class=""ic-star"">
                                                    <i class=""fas fa-star-sharp""></i>
                                                </div>";
            }
            htmlProduct += @"</div>
                                        </div>";
            if (countvote > 0)
            {
                htmlProduct += @"<div class=""label-rate text-primary"">
                                            (" + countvote + @" <span>đánh giá </span>)                                               
                                        </div>";
            }
            htmlProduct += @"</div>
                                    <div class=""block-box-price"">
                                        <div class=""box-info__box-price"">
                                            <p class=""product__price--show"">";
            if (lstProduct[i].PriceView > 0)
            {
                htmlProduct += Utils.FormatMoney(lstProduct[i].PriceView) + "đ";
            }
            else
            {
                htmlProduct += "Liên hệ";
            }
            htmlProduct += "</p>";
            if (lstProduct[i].PriceView2 > 0)
            {
                htmlProduct += @"<p class=""product__price--through"">
                                                " + Utils.FormatMoney(lstProduct[i].PriceView2) + @"đ                                                   
                                            </p>";
                if (lstProduct[i].SellOffPercent > 0)
                {
                    htmlProduct += @"<div class=""product__price--percent"">
                                                <p class=""product__price--percent-detail"">
                                                    Giảm&nbsp;" + lstProduct[i].SellOffPercent + @"%                                                       
                                                </p>
                                            </div>";
                }
            }
            htmlProduct += @"</div>
                                    </div>
                                    </a>
                                                                </div>
                                                                <a class=""block-add-to-cart addToCartAjax"" data-id=""" + lstProduct[i].ID + @""" data-hasproperty=""" + hasPropertyItem.ToString().ToLower() + @""" href=""javascript:void(0)"">Thêm giỏ hàng
                                                                </a>
                                    <div class=""install-0-tag"">
                                        Trả góp 0%
                                    </div>
                                    </div>
                                                        </div>";
            htmlProduct += @"</div>";
        }
        htmlProduct += @"</div>";
        htmlProduct += @"</div>";
    }
    if (content.Contains(@"listProductForNews"))
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(content);
        if (doc != null)
        {
            var listNode = doc.DocumentNode.SelectNodes(@"//div[contains(@class, 'listProductForNews')]");
            for (int i = 0; listNode != null && i < listNode.Count; i++)
            {
                listNode[i].SetAttributeValue("class", "");
                listNode[i].InnerHtml = htmlProduct;
            }
            content = doc.DocumentNode.InnerHtml;
        }
    }
%>
<div class="article-detail">
    <div class="dok-heading">
        <h1 class="mb-0"><%=item.Name %></h1>
    </div>
    <div class="authorBox">
        <%if (item.AuthorID > 0)
            {
                var author = item.GetAuthor();
        %>
        <span class="authorName">Biên tập bởi <b><a href="<%=ViewPage.GetURL("author/"+author.Code) %>"><%=author.Name %></a></b></span>
        <%} %>
        <span><i class="far fa-calendar-clock me-2"></i><%=Utils.FormatDate(item.Published) %></span>
        <%if (item.View > 0)
            { %>
        <span><i class="far fa-light fa-eye me-2"></i><%=Utils.FormatMoney(item.View) %> lượt xem</span>
        <%} %>
        <%if (item.MenuID > 0)
            {
                var pageMenu = SysPageService.Instance.CreateQuery().Where(o => o.MenuID == item.MenuID && o.ModuleCode == "MNews" && o.Activity == true).ToSingle_Cache();
                if (pageMenu != null)
                {
        %>
        <span><i class="far fa-tag me-2"></i>
            <a href="<%=ViewPage.GetPageURL(pageMenu) %>" class="text-link"><%=item.GetMenu().Name %></a>
        </span>
        <%
                }
            }
        %>
    </div>
    <hr>

    <div class="article-content">
        <%=content %>
    </div>
    <%if (ArrTag != null && ArrTag.Length > 0 && !string.IsNullOrEmpty(item.Tags.Trim()))
        { %>
    <div class="tags">
        <strong class="fz-18">Từ khóa:</strong>
        <%for (int i = 0; ArrTag != null && i < ArrTag.Length; i++)
            {
                ArrTag[i] = ArrTag[i].Trim();
        %>
        <%if (i > 0)
            { %>,&nbsp;<%} %><a href="<%= ViewPage.GetURL("ntag/"+Data.GetCode(ArrTag[i])) %>" title="<%=ArrTag[i] %>"><%=ArrTag[i] %></a>,
        <%} %>
    </div>
    <%} %>
</div>
<hr class="mobile-line" />

<%if (item.AuthorID > 0)
    {
        var author = item.GetAuthor();
%>
<div class="dok-widget">
    <div class="author-box flex-row align-items-center mt-0">
        <div class="author-avatar me-3">
            <img src="<%=Utils.GetWebPFile(author.File, 4, 115,115) %>" alt="<%=author.Name %>" class="img-fluid w-100 h-100">
        </div>
        <div class="d-flex align-items-end w-100 mb-3 ms-4">
            <div class="author-info">
                <p class="fz-18 fw-bold mb-1"><%=author.Name %></p>
                <p class="fz-14 mb-0 text-secondary">
                    <%=author.Position %>
                </p>
            </div>
        </div>
    </div>
</div>
<hr class="mobile-line" />
<%} %>
<%if (listItem != null && listItem.Count > 0)
    { %>
<div class="dok-widget">
    <h2 class="fw-bold fz-18 mb-3">Bài viết liên quan</h2>
    <div class="dok-news mb-4">
        <div class="d-flex flex-column">
            <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    string url = ViewPage.GetURL(listItem[i].Code);
            %>
            <div class="item news-item-small">
                <div class="img">
                    <a href="<%=url %>" title="<%=listItem[i].Name %>">
                        <img class="img-fluid"
                            src="<%=Utils.GetWebPFile(listItem[i].File, 4 , 144, 80) %>"
                            alt="<%=listItem[i].Name %>">
                    </a>
                </div>
                <div class="wrapper">
                    <div class="name">
                        <a href="<%=url %>" title="<%=listItem[i].Name %>"><%=listItem[i].Name %></a>
                    </div>
                    <div class="meta">
                        <%if (listItem[i].AuthorID > 0)
                            {
                                var author = listItem[i].GetAuthor();
                        %>
                        <a href="<%=ViewPage.GetURL("author/"+author.Code) %>">
                            <i class="fa fa-user me-2"></i><%=author.Name %>
                                </a>
                        <span class="mx-3">|</span>
                        <%} %>
                        <span>
                            <i class="fa fa-calendar-alt me-2"></i><%=Utils.FormatDate(listItem[i].Published) %>
                                </span>
                    </div>
                </div>
            </div>

            <%} %>
        </div>
    </div>
    <a class="readmore-btn mb-0" href="<%=ViewPage.GetPageURL(ViewPage.CurrentPage) %>">
        <span>Xem thêm bài viết</span>
    </a>
</div>
<hr class="mobile-line" />
<%} %>
