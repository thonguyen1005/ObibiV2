<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<div class="page-content-wrapper">
    <h3 class="page-title">Quản lý File</h3>
    <div class="page-bar justify-content-between">
        <ul class="breadcrumb">
            <li class="breadcrumb-item">
                <i class="fa fa-home"></i>
                <a href="/{CPPath}/">Home</a>
            </li>
            <li class="breadcrumb-item">
                <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Quản lý File</a>
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
            <div class="portlet portlet">
                <div class="portlet-body">
                    <div class="dataTables_wrapper">
                        <div class="table-scrollable">

                            <script type="text/javascript">
                                function showFileInfo(fileUrl, data) {
                                    var msg = "The selected URL is: <a href=\"" + fileUrl + "\">" + fileUrl + "</a><br /><br />";
                                    if (fileUrl !== data["fileUrl"])
                                        msg += "<b>File url:</b> " + data["fileUrl"] + "<br />";
                                    msg += "<b>File size:</b> " + data["fileSize"] + "KB<br />";
                                    msg += "<b>Last modified:</b> " + data["fileDate"];

                                    this.openMsgDialog("Selected file", msg);
                                }

                                var finder = new CKFinder();
                                finder.basePath = "../";
                                finder.height = 600;
                                finder.selectActionFunction = showFileInfo;
                                finder.create();
                            </script>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>