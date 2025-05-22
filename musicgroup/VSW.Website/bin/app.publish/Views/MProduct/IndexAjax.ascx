<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<%
    var listItem = ViewBag.Data as List<ModProductEntity>;
%>
<%for (var i = 0; listItem != null && i < listItem.Count; i++)
    {
        string url = ViewPage.GetURL(listItem[i].MenuID, listItem[i].Code);
        var countvote = ModVoteService.Instance.CreateQuery().Where(o => o.ProductID == listItem[i].ID && o.Type == "Product" && o.Activity == true).Count().ToValue_Cache().ToInt();
        var countBuy = VSW.Core.Global.DAO<GetSumModel>.Populate(ModOrderDetailService.Instance.ExecuteReader("select sum([Quantity]) from " + ModOrderDetailService.Instance.TableName + " where [ProductID] = " + listItem[i].ID));
        var hasProperty = ModProductClassifyService.Instance.CreateQuery().Where(o => o.ProductID == listItem[i].ID).Count().ToValue_Cache().ToBool();
%>
<div class="product">
    <div class="product-info-container">
        <div class="product-info">
            <a href="<%=url %>" title="<%=listItem[i].Name %>" class="product__link button__link" rel="">
                <div class="product__image">
                    <img src="<%=Utils.GetWebPFile(listItem[i].File, 4, 358, 358) %>"
                        width="358" height="358"
                        alt="<%=listItem[i].Name %>"
                        class="product__img">
                </div>
                <div class="product__name">
                    <h3 class="mb-0"><%=listItem[i].Name %></h3>
                </div>
                <%if (countBuy != null && countBuy.Count > 0)
                    { %>
                <div class="block-smem-price"><%=countBuy[0].Total %> sản phẩm đã bán</div>
                <%} %>
                <div class="bottom-div">
                    <div class="product__box-rating">
                        <div>
                            <%for (int z = 0; z < listItem[i].Star; z++)
                                {%>
                            <div class="ic-star is-active">
                                <i class="fas fa-star-sharp"></i>
                            </div>
                            <%} %>
                            <%for (int z = 0; z < 5 - listItem[i].Star; z++)
                                {%>
                            <div class="ic-star">
                                <i class="fas fa-star-sharp"></i>
                            </div>
                            <%} %>
                        </div>
                    </div>
                    <%if (countvote > 0)
                        { %>
                    <div class="label-rate text-primary">
                        (<%=countvote %> <span>đánh giá </span>)                                               
                    </div>
                    <%} %>
                </div>
                <div class="block-box-price">
                    <div class="box-info__box-price">
                        <p class="product__price--show">
                            <%if (listItem[i].PriceView > 0)
                                { %>
                            <%=Utils.FormatMoney(listItem[i].PriceView) %>đ
                                                        <%}
                                                            else
                                                            { %>{RS:Lienhe}
                                                        <%} %>
                        </p>
                        <%if (listItem[i].PriceView2 > 0)
                            { %>
                        <p class="product__price--through">
                            <%=Utils.FormatMoney(listItem[i].PriceView2) %>đ                                                   
                        </p>
                        <%if (listItem[i].SellOffPercent > 0)
                            { %>
                        <div class="product__price--percent">
                            <p class="product__price--percent-detail">
                                Giảm&nbsp;<%=listItem[i].SellOffPercent %>%                                                       
                            </p>
                        </div>
                        <%} %>
                        <%} %>
                    </div>
                </div>
            </a>
        </div>
        <a class="block-add-to-cart addToCartAjax" data-id="<%=listItem[i].ID %>" data-hasproperty="<%=hasProperty.ToString().ToLower() %>" href="javascript:void(0)">Thêm giỏ hàng
        </a>
        <div class="install-0-tag">
            Trả góp 0%
        </div>
    </div>
</div>
<%} %>