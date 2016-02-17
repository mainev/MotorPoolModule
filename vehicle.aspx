<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="vehicle.aspx.cs" Inherits="vehicle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- DataTables CSS -->
    <link href="bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.css" rel="stylesheet" />
    <!-- DataTables Responsive CSS -->
    <link href="bower_components/datatables-responsive/css/dataTables.responsive.css" rel="stylesheet" />
    <!-- JQuery Confirm CSS -->
    <link href="bower_components/jquery-confirm/jquery-confirm.min.css" rel="stylesheet" />

    <style>
        div.dataTables_scrollHead thead tr {
            height: 30px;
        }

        .tooltip {
            border: 2px solid blue;
            display: none;
        }

        img {
            height: 300px;
            width: 100%;
            max-width: 300px;
            max-height: 200px;
        }
    </style>


    <div class="row">
        <div class="col-lg-12">
            <div>
                <ul class="my-header-ul">
                    <li>
                        <h3><i class="fa fa-bar-chart fa-fw"></i>Vehicle </h3>
                    </li>
                    <li>
                        <h3><span class="sub-header">
                            <asp:Label ID="lblMenu" runat="server" Text=""></asp:Label></span></h3>
                    </li>
                </ul>
            </div>
        </div>
    </div>


    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="ViewButtons" runat="server">
            <div class="col-lg-12">
                <div class="row xrow">
                    <div class="btn-toolbar my-toolbar" role="toolbar">
                        <div class="btn-group btn-group-sm">
                            <button id="btn_Add" type="button" class="btn btn-primary"><span class="fa fa-plus fa-fw" aria-hidden="true"></span>Add</button>
                        </div>
                        <div class="btn-group btn-group-sm">
                            <button id="btn_Save" type="button" class="btn btn-primary disabled"><span class="fa fa-floppy-o fa-fw" aria-hidden="true"></span>Save</button>
                            <button id="btn_Cancel" type="button" class="btn btn-primary disabled"><span class="fa fa-remove fa-fw" aria-hidden="true"></span>Cancel</button>
                        </div>
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
        </asp:View>
        <asp:View ID="ViewNothing" runat="server">
        </asp:View>
    </asp:MultiView>

    <asp:MultiView ID="MultiView2" runat="server">

        <asp:View ID="View_Entry" runat="server">


            <div id="input_fields" class="row xrow ">
                <div class="col-sm-4">
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Name : </span>
                            <input type="text" id="input_name" class="form-control" disabled required maxlength="50" placeholder="Name"  />

                        </div>
                    </div>

                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Description : </span>
                            <input type="text" id="input_descs" class="form-control" disabled required maxlength="255" placeholder="Description" />
                        </div>
                    </div>
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Model - Year : </span>
                            <input type="text" id="input_model_yr" class="form-control" disabled required maxlength="20" placeholder="Model - Year" />
                        </div>
                    </div>

                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Color : </span>
                            <input type="text" id="input_color" class="form-control" disabled required maxlength="10" placeholder="Color" />
                        </div>
                    </div>
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Plate No. : </span>
                            <input type="text" id="input_plate_no" class="form-control" disabled required maxlength="10" placeholder="Plate No."  />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Insurance Po. No. : </span>
                            <input type="text" id="input_insu_po_no" class="form-control" disabled required maxlength="10" placeholder="Insurance Policy No."  />
                        </div>
                    </div>

                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Seater Cap. : </span>
                            <input type="text" id="input_seater_cap" class="form-control money-align" disabled required  placeholder="Seater Capacity"  />
                        </div>
                    </div>
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Fuel Type : </span>
                            <input type="text" id="input_fuel_type" class="form-control" disabled required maxlength="10" placeholder="Fuel Type"  />
                        </div>
                    </div>
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs">
                            <span class="input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Location : </span>
                            <input type="text" id="input_location" class="form-control" disabled required maxlength="50" placeholder="Location"  />
                        </div>
                    </div>
                    <div id="" class=" row xrow">
                        <div class="input-group input-group-xs" id="unified-inputs">
                            <span class="hidden input-group-addon span-addon-pw" style="color: #286090; text-align: left;">Distance / Fuel Unit: </span>

                            <input type="hidden" id="input_distance_u" class="form-control" disabled readonly required maxlength="255" placeholder="Description" value="KM" />
                            <input type="hidden" id="input_fuel_u" class="form-control" disabled readonly required maxlength="255" placeholder="Description" value="Litre" />

                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <img id="imageid" src="images/photo.png" class="img-thumbnail" alt="Upload Photo"  /><p></p>
                    <input type="file" id="image" disabled />
                </div>

            </div>

        </asp:View>
    </asp:MultiView>

    <asp:MultiView ID="MultiView3" runat="server">


        <asp:View ID="View_Table" runat="server">
            <table id="tbl_vehicle" class="table-bordered table-hover item_report table-font">
                <thead class="GridHeader" />
            </table>
        </asp:View>
    </asp:MultiView>

    <!-- DataTables JavaScript -->

    <script src="bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>


    <!-- MaskEdit JavaScript -->
    <script src="bower_components/maskedit/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="bower_components/maskedit/jquery.price_format.2.0.min.js" type="text/javascript"></script>

  

    <script src="javascript/my_validator.js" type="text/javascript" lang="javascript"></script>
    <script src="javascript/vehicle.js" type="text/javascript" lang="javascript"></script>
</asp:Content>

