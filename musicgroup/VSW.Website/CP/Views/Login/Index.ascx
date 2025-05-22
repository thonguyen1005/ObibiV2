<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>
<div class="container-xxl">
    <div class="authentication-wrapper authentication-basic container-p-y">
        <div class="authentication-inner">
            <!-- Register -->
            <div class="card">
                <div class="card-body">
                    <!-- Logo -->
                    <div class="app-brand justify-content-center">
                        <img src="/{CPPath}/Content/skins/img/logo.png" style="width: 100px;" alt="logo" />
                    </div>
                    <form method="post" id="loginForm" name="loginForm" class="mb-3">
                        <div class="mb-3">
                            <label for="UserName" class="form-label">{RS:Login_LoginTitle}</label>
                            <input type="text" class="form-control" id="UserName" name="UserName" onkeydown="if (event.keyCode == 13) { loginForm.submit(); }"  autofocus />
                        </div>
                        <div class="mb-3 form-password-toggle">
                            <div class="d-flex justify-content-between">
                                <label class="form-label" for="Password">{RS:Login_Password}</label>
                            </div>
                            <div class="input-group input-group-merge">
                                <input type="password" id="Password" class="form-control" name="Password" onkeydown="if (event.keyCode == 13) { loginForm.submit(); }"  />
                                <span class="input-group-text cursor-pointer"><i class="bx bx-hide"></i></span>
                            </div>
                        </div>
                        <div class="mb-3 form-password-toggle">
                            <div class="d-flex justify-content-between">
                                <label class="form-label" for="lang">{RS:Login_Language}</label>
                            </div>
                            <div class="input-group input-group-merge">
                                <select id="lang" name="lang" class="form-select" onchange="document.getElementById('_vsw_action').value='[ChangeLang]['+this.value+']';loginForm.submit();">
                                    <option <%if (CPViewPage.CurrentLang.Code == "vi-VN")
                                        {%>
                                        selected <%} %> value="vi-VN">Việt Nam</option>
                                    <option <%if (CPViewPage.CurrentLang.Code == "en-US")
                                        {%>
                                        selected <%} %> value="en-US">English</option>
                                </select>
                            </div>
                        </div>
                        <%= ShowMessage()%>
                        <div class="mb-3">
                            <button class="btn btn-primary d-grid w-100" type="button" onclick="loginForm.submit();">{RS:Login_LoginSubmit}</button>
                        </div>
                        <input type="hidden" id="_vsw_action" name="_vsw_action" value="Login" />
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
