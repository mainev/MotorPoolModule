<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MotorPoolMasterfile.aspx.cs" Inherits="MotorPoolMasterfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
   
   
   

    <div class="row">
        <div class="col-lg-12">
            <div>
                <ul class="my-header-ul">
                    <li>
                        <h3><i class="fa fa-cogs fa-fw"></i>Motor Pool Masterfiles  </h3>
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

    <asp:MultiView ID="MultiView1" runat="server">
       <asp:View ID="ViewButtons" runat="server"> 
            <div class="col-lg-12">
                <div class="row xrow">
                    <div class="btn-toolbar my-toolbar" role="toolbar">
                        <div class="btn-group btn-group-sm">
                            <button id="btnAdd" type="button" class="btn btn-primary"><span class="fa fa-plus fa-fw" aria-hidden="true"></span> Add</button>
                        </div>
                        <div class="btn-group btn-group-sm">
                            <button id="btnSave" type="button" class="btn btn-primary disabled"><span class="fa fa-floppy-o fa-fw" aria-hidden="true"></span> Save</button>
                            <button id="btnCancel" type="button" class="btn btn-primary disabled"><span class="fa fa-remove fa-fw" aria-hidden="true"></span> Cancel</button>
                        </div>
                        <div id="xsearch_mf" class="input-group input-group-sm pull-right">
                            <span class="input-group-addon"><span class="fa fa-search fa-fw"></span></span>
                            <input type="text" id="txtsearch" class="form-control" placeholder="Search" />
                        </div>
                    </div>
                </div>
            </div>
           
        </asp:View>
        <asp:View ID="ViewNothing" runat="server"> 
        </asp:View>
    </asp:MultiView>

    <asp:MultiView ID="MultiView2" runat="server">
        <asp:View ID="ViewAccessory" runat="server">                     
            <div class="row xrow">
                <div class="col-sm-4">                  
                    <div class="row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Code : </span>
                            <input type="text" id="va_code" class="form-control" value="" placeholder="Code" disabled autofocus required maxlength="10" />
                        </div>
                    </div>
                    <div class="row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Description :* </span>
                            <input type="text" id="va_description" class="form-control" value="" placeholder="Description" disabled autofocus required maxlength="255" />
                        </div>
                    </div>
                    <div class="row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Group : </span>
                            <asp:DropDownList ID="va_DD_accessory_group" CssClass="form-control" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row xrow">
                <table id="T_Accessory" class=" table-bordered table-hover item_inventory table-font dataTables_scrollBody" style="height: 20px; overflow-y: auto;">
                    <thead class="GridHeader">
                        <%--<tr>
                            <td>Code</td>
                            <td>Description</td>
                            <td>Group Code</td>                           
                        </tr>--%>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
            <script src="javascript/masterfileAccessory.js"></script>
         </asp:View>

        <asp:View ID="ViewAccessoryGroup" runat="server">                     
            <div class="row xrow">
                <div class="col-sm-4">
                  
                    <div class="row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Code : </span>
                            <input type="text" id="txt_code" class="form-control" value="" placeholder="Code" disabled autofocus required maxlength="10" />
                        </div>
                    </div>
                    <div class="row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Description :* </span>
                            <input type="text" id="txt_description" class="form-control" value="" placeholder="Description" disabled autofocus required maxlength="255" />
                        </div>
                    </div>                    
                </div>
            </div> 

             <div class="row xrow">
                <table id="T_Accessory_Group" class=" table-bordered table-hover item_inventory table-font dataTables_scrollBody" style="height: 20px; overflow-y: auto;">
                    <thead class="GridHeader">
                    </thead>
                </table>
            </div>
            <script src="javascript/masterfileAccessoryGroup.js"></script>
        </asp:View>

        <asp:View ID="ViewVehicleAccessory" runat="server">
            <div class="row xrow">
                <div class="col-sm-4">
                </div>
            </div>
            <div class="row xrow">
                <table class="table">
                    <thead>
                        <tr>
                            <th>
                                <div class="row xrow">
                                    <div class="input-group input-group-xs">
                                        <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Vehicle : </span>
                                        <asp:DropDownList ID="DD_vehicle_plate_number" CssClass="form-control" runat="server" Enabled="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </th>
                        <th style="min-width: 80px;">&nbsp;</th>
                        <th>
                            <div class="row xrow">
                                <div class="col-sm-6">
                                    <div class="input-group input-group-xs">
                                        <span class="input-group-addon span-addon" style="color: #286090; text-align: left;">Accesory Group : </span>
                                        <asp:DropDownList ID="DD_accessory_group" CssClass="form-control" runat="server" Enabled="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div id="" class="input-group input-group-xs">
                                        <span class="input-group-addon"><span class="fa fa-search fa-fw"></span></span>
                                        <input type="text" id="search" placeholder="search" class="CssClass form-control" />                                      
                                    </div>
                                </div>
                            </div>
                        </th>
                    </tr>
                </thead>
                <tbody style="height: 200px;">
                    <tr>
                        <td style="width: 500px">                            
                            <asp:Panel ID="pnl_App2" runat="server" ScrollBars="Auto" Height="450px" Width="100%">
                                <table id="T_Vehicle_Accessory" class="table table-bordered table-hover item_inventory table-font">
                                    <thead class="GridHeader">
                                        <tr>
                                            <th><input type="checkbox" name="xx" runat="server" /></th>
                                            <th>Accessory Group</th>
                                            <th>Accessory</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <asp:Button ID="btnCheck2" Style="visibility: hidden;" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="min-width: 80px;">
                            <button type="button" id="btnSelect" class="btn btn-sm btn-info btn-block">< Add</button>
                            <button type="button" id="btnUnSelect" class="btn btn-sm btn-danger btn-block">Remove ></button>
                        </td>
                        <td style="width: 500px">   
                            <asp:Panel ID="pnl_App1" runat="server" ScrollBars="Auto" Height="450px" Width="100%">
                                <table id="T_Accessories" class="table table-bordered table-hover item_inventory table-font">
                                    <thead class="GridHeader">
                                        <tr>
                                            <th>
                                               <%-- <input type="checkbox" id="selectAll"  name="xx" runat="server" />--%>
                                            </th>
                                            <th>Code </th>
                                            <th>Description</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <asp:Button ID="btnCheck" Style="visibility: hidden;" runat="server" />
                            </asp:Panel>

                        </td>
                    </tr>
                </tbody>
                </table>
            </div>
            <script src="javascript/masterfileVehicleAccessory.js"></script>
        </asp:View>
    </asp:MultiView>

  

     <!-- DataTables JavaScript -->

    <script src="bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>


    <input id="hidden_entry_new" type="hidden" />
    <input id="hidden_username" type="hidden" runat="server" />

   <%-- <script type="text/javascript">
    var CurrentView = <%= MultiView2.ActiveViewIndex %>
     alert("Current Active View Index Is :"+CurrentView);
</script>--%>

</asp:Content>

