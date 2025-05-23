﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModTagModel;
    var item = ViewBag.Data as ModTagEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <div class="page-content-wrapper">
        <h3 class="page-title">Tags <small><%=model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Tags</a>
                </li>
            </ul>
            <div class="page-toolbar">
                <div class="btn-group">
                    <a href="/" class="btn green" target="_blank"><i class="icon-screen-desktop"></i>Xem Website</a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>

                <div class="form-horizontal form-row-seperated">
                    <div class="portlet">
                        <div class="portlet-title">
                            <div class="caption"></div>
                            <div class="actions btn-set">
                                <%= GetDefaultAddCommand()%>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-group row">
                                                <label class="col-md-3 col-form-label text-right">Tên:</label>
                                                <div class="col-md-9">
                                                    <input type="text" class="form-control title" name="Name" id="Name" value="<%=item.Name %>" />
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-md-3 col-form-label text-right">URL trình duyệt:</label>
                                                <div class="col-md-9">
                                                    <input type="text" class="form-control" name="Code" value="<%=item.Code %>" placeholder="Nếu không nhập sẽ tự sinh theo Tiêu đề" />
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-md-3 col-form-label text-right">Link:</label>
                                                <div class="col-md-9">
                                                    <input type="text" class="form-control title" name="Link" id="Link" value="<%=item.Link %>" />
                                                </div>
                                            </div>
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-12 col-form-label text-right">[SEO] Tiêu đề</label>
                                                    <div class="col-md-12">
                                                        <textarea class="form-control ckeditor" name="Title" id="Title" rows="" cols=""><%=item.Title %></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-12 col-form-label text-right">[SEO] Mô tả </label>
                                                    <div class="col-md-12">
                                                        <textarea class="form-control ckeditor" name="Description" id="Description" rows="" cols=""><%=item.Description%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-12 col-form-label text-right">[SEO] Từ khóa tìm kiếm</label>
                                                    <div class="col-md-12">
                                                        <textarea class="form-control ckeditor" name="Keywords" id="Keywords" rows="" cols=""><%=item.Keywords %></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>
