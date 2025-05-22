<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<%
    var listItem = ViewBag.Data as List<ModFAQEntity>;
    string Title = ViewBag.Title;
    if (listItem == null) return;
%>
<hr class="mobile-line" />

<div class="dok-faqs mb-4">
    <h2 class="title">Câu hỏi thường gặp</h2>
    <div class="accordion" id="">
        <%for (int i = 0; listItem != null && i < listItem.Count; i++)
            {%>
        <div class="mb-1">
            <button class="b-button collapsed" type="button" data-bs-toggle="collapse"
                data-bs-target="#fq-<%=i+1 %>">
                <h3><%=listItem[i].Name %></h3>
                <div class="icon">
                    <i class="fas fa-angle-right"></i>
                </div>
            </button>
            <div id="fq-<%=i+1 %>" class="accordion__content collapse">
                <div class="content-wrapper">
                    <%=listItem[i].Content %>
                </div>
            </div>
        </div>
        <% } %>
    </div>
</div>
