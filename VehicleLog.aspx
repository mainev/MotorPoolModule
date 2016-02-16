<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleLog.aspx.cs" Inherits="VehicleLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script src="javascript/VehicleLog.js"></script>

     <div class="row">
        <div class="col-lg-12">
            <div>
                <ul class="my-header-ul">
                    <li>
                        <h3><i class="fa fa-bar-chart fa-fw"></i>Vehicle Log </h3>
                    </li>
                    <li>
                        <h3><span class="sub-header">
                            <asp:Label ID="lblMenu" runat="server" Text=""></asp:Label></span></h3>
                    </li>
                </ul>
            </div>
        </div>
        <!-- /.col-lg-12 -->
    </div>


    <div class="col-lg-12">
        <div class="row xrow">
            <div class="btn-toolbar my-toolbar" role="toolbar">
                <div class="btn-group btn-group-sm">
                    <button id="btnAdd" type="button" class="btn btn-primary"><span class="fa fa-plus fa-fw" aria-hidden="true"></span>Add</button>
                </div>
                <div class="btn-group btn-group-sm">
                    <button id="btnSave" type="button" class="btn btn-primary disabled"><span class="fa fa-floppy-o fa-fw" aria-hidden="true"></span>Save</button>
                    <button id="btnCancel" type="button" class="btn btn-primary disabled"><span class="fa fa-remove fa-fw" aria-hidden="true"></span>Cancel</button>
                </div>
                <%--<div class="btn-group btn-group-sm">
                            <button id="btnOption" type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <span class="fa fa-hand-o-right fa-fw" aria-hidden="true"></span>Option <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a href="#" id="link_print"><span class="fa fa-print fa-fw" aria-hidden="true"></span>Reports</a></li>
                                <li id="li_email" class="disabled"><a href="#"><span class="fa fa-paper-plane fa-fw" aria-hidden="true"></span>E-mail</a></li>
                                <li role="separator" class="divider"></li>
                                <li id="li_approved" class="disabled"><a href="#"><span class="fa fa-thumbs-o-up fa-fw" aria-hidden="true"></span>Approve</a></li>
                                <li id="li_disapproved" class="disabled"><a href="#"><span class="fa fa-thumbs-o-down fa-fw" aria-hidden="true"></span>Disapprove</a></li>
                            </ul>
                        </div>--%>
                <div class="pull-right">
                    <div id="xsearch_mf" class="input-group input-group-sm">
                        <span class="input-group-addon"><span class="fa fa-search fa-fw"></span></span>
                        <input type="text" id="txtsearch" class="form-control" placeholder="Search" />
                    </div>
                    <div id="search_document" class="list-group search_hide"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="row xrow">
        <div class="col-sm-4">
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Vehicle : </span>
                    <asp:DropDownList ID="DD_vehicle_plate_number" CssClass="form-control" runat="server" Enabled="false">
                       <%-- <asp:ListItem Value="WSS-0922" Selected="True">Isuzu Cross Wind</asp:ListItem>
                        <asp:ListItem Value="RWE-9831">Mitsubishi L-300</asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Driver : </span>
                    <asp:DropDownList ID="DD_driver_username" CssClass="form-control" runat="server" Enabled="false">
                        
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Odometer : </span>
                    <input type="text" id="odometer" runat="server" class="form-control money-align" value="0" placeholder="Odometer" disabled  autofocus required maxlength="20" />
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Last Fuel Price : </span>                                                                    
                    <input type="text" id="last_fuel_price" runat="server" class="form-control money-align"   placeholder="Last Fuel Price" disabled autofocus readonly required maxlength="250" />
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <asp:RadioButtonList ID="RB_full_tank" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Value="True" Selected="True">Full Tank&nbsp;</asp:ListItem>
                        <asp:ListItem Value="False">Partial</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>           
        </div>

        <div class="col-sm-4">
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Date : </span>
                    <input type="text" id="date" runat="server" class="form-control" placeholder="Date" disabled required maxlength="250" />
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Total Gas : </span>
                    <input type="text" id="total_gas" runat="server" class="form-control money-align" value="0" placeholder="Total Gas" disabled required maxlength="250" />
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Total Cost : </span>
                    <input type="text" id="total_cost" runat="server" class="form-control money-align" value="0" placeholder="Total Cost" disabled required maxlength="20" />
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Result : </span>
                    <input type="text" id="result" runat="server" class="form-control" value="0" placeholder="Result" disabled readonly required maxlength="250" />
                </div>
            </div>
        </div>

        <div class="col-sm-4">
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Audit User : </span>
                    <input type="text" id="audit_user" runat="server" class="form-control" placeholder="Audit User" disabled required maxlength="250" />
                </div>
            </div>
            <div class="row xrow">
                <div class="input-group input-group-xs">
                    <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Audit Date : </span>
                    <input type="text" id="audit_date" runat="server" class="form-control" placeholder="Audit Date" disabled required maxlength="250" />
                </div>
            </div>
        </div>
    </div>

    <div class="row xrow">
        <table id="T_VehicleLog" class="table table-bordered table-hover item_inventory table-font dataTables_scrollBody" style="height:20px ; overflow-y: auto;">
            <thead class="GridHeader">
                <tr>
                    <th>Plate Number</th>
                    <th>Odometer</th>
                    <th>Last Fuel Price</th>
                    <th>Total Gas</th>
                    <th>Total Cost</th>
                    <th>Date</th>
                    <th>Full Tank</th>
                    <th>Result (Km/L)</th>
                    <th>Driver Username</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>    

    <!-- MaskEdit JavaScript -->
    <script src="bower_components/maskedit/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="bower_components/maskedit/jquery.price_format.2.0.min.js" type="text/javascript"></script>

</asp:Content>

