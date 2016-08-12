<%@ Page Language="C#" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Formulary / Pharmaceuticals Lookup</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE;" />
    <link href="js/vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="js/vendors/jquery-ui/jquery-ui.min.css" type="text/css" rel="stylesheet" />
    <link href="js/vendors/dragtable/dragtable.css" type="text/css" rel="stylesheet" />
    <link href="css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="js/vendors/jquery/jquery-2.2.0.min.js"></script>
    <script type="text/javascript" src="js/vendors/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="js/vendors/jquery-ui/jquery-ui-1.11.4.min.js"></script>
    <script src="js/vendors/angular-1.5.0/angular.min.js"></script>
    <script src="js/vendors/angular-1.5.0/angular-sanitize.min.js"></script>
    <script src="js/vendors/exporttoexcel/xlsx.core.min.js"></script>
    <script src="js/vendors/exporttoexcel/Blob.js"></script>
    <script src="js/vendors/exporttoexcel/FileSaver.js"></script>
    <script src="js/vendors/exporttoexcel/Export2Excel.js"></script>
    <script src="js/vendors/dragtable/jquery.dragtable.js"></script>
    <script type="text/javascript" src="js/default.js"></script>   
    <style type="text/css" media="print">
        .no-print { display: none; }
    </style>
    <script>
		function CheckBrowser() {
			var is_chrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
			if (!is_chrome) {
				window.location = "ChromeRedirect.aspx";
			}
		}

        $(document).ready(function () {
			CheckBrowser();

            //Freeze table header
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 326) {
                    $('#draggable_header').addClass('fixed');
                    for (var i = 1; i <= 24; i++) {
                        $('#Th' + i).width($('#d' + i).width());
                        $('#d' + i).width($('#Th' + i).width());
                    };
                }
                else {
                    $('#draggable_header').removeClass('fixed');
                }
            });
            
           
            //Trigger search when user presses Enter key
            $("#frmSearch").on('keypress.enter', 'input[type=text]', function (e) {
                if ((e.keyCode == 13)) {
                    e.preventDefault();
                    $('#search').trigger('click');
                }
            });
            //$("html, body").animate({ scrollTop: 0 }, "slow");
            $("a[href='#top']").click(function () {
                $("html, body").animate({ scrollTop: 0 }, "slow");
                return false;
            });


            //Make the data columns draggable
            $('#pharma_reportTable').dragtable({
                dragHandle: '.dragtable-handle'
            });

            $('.dragtable-handle').attr('title', 'Click & drag');
        });
    </script>  
</head>
<body id="pharma" data-ng-app="pharmaLookup"  data-ng-controller="PharmaCtrl" data-ng-init="init()" >  
    <div class="content-fluid no-print" >
        <div id="panel" >
            <span id="title" style="color:#3b5998">Formulary / Pharmaceuticals </span> <span style="font-size:20px;color:#8b9dc3;font-weight:700"> Lookup</span>
            <span style="float: right;margin-top: 10px; font-size: 14px">Lookup pharmaceuticals by Therapeutic Class, NDC, GPI, Brand, Product, or Manufacturer.</span>  
            <hr style="margin-top:2px;margin-bottom:6px"/>
            <div class="row">
                <div class="col-sm-4">
                    <div class="row rowSearch_SearchType" data-ng-init="radioSelected='PRODUCT'">
                        <label class="col-xs-3 control-label" for="NDC" style="color: navy;text-align:right">SEARCH BY:</label>
                        <div class="col-xs-9" style="line-height:18px">
                            <div>
                                <label for="rda_search_type_PRODUCT" class="radio-inline">
                                <input name="search_type" id="rda_search_type_PRODUCT" type="radio" data-ng-click="changeRadioSelection('PRODUCT')" checked="checked" value="PRODUCT"  />Product Name</label>
                            </div>
                             <div>
                                <label for="rda_search_type_PHARMCLASS" class="radio-inline">
                                <input name="search_type" id="rda_search_type_PHARMCLASS" type="radio" data-ng-click="changeRadioSelection('PHARMCLASS')"  value="PHARMCLASS"  />
                                Therapeutic Group / Class Lookup</label>
                            </div>
                            <div>
                                <label for="rda_search_type_SELECTBOX" class="radio-inline">
                                <input name="search_type" id="rda_search_type_SELECTBOX" type="radio" data-ng-click="changeRadioSelection('SELECTBOX')" value="SELECTBOX"  />Therapeutic Group / Class Selection</label>
                            </div>
                           
                            <div>
                                <label for="rda_search_type_MANUFACTURER" class="radio-inline">
                                <input name="search_type" id="rda_search_type_MANUFACTURER" type="radio" data-ng-click="changeRadioSelection('MANUFACTURER')"  value="MANUFACTURER"  />Manufacturer</label>
                            </div>
                            <div>
                                <label for="rda_search_type_NDC" class="radio-inline">
                                <input name="search_type" id="rda_search_type_NDC" type="radio" data-ng-click="changeRadioSelection('NDC')"  value="NDC"  />NDC</label>
                            </div>
                            <div>
                                <label for="rda_search_type_GPI" class="radio-inline">
                                <input name="search_type" id="rda_search_type_GPI" type="radio" data-ng-click="changeRadioSelection('GPI')"  value="GPI" />GPI</label>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-12" style="text-align: center;margin-bottom:5px;margin-top:10px">
                        <div class="row">
                            <button id="excel" class="btn btn-default" title="Export to Excel" style="color:green;width:40px;"
                                data-ng-click="exportToExcel('pharma_reportTable');" data-ng-show="showReport"  ><i class=" fa fa-file-excel-o fa-lg"></i></button>    
                            <button id="print" class="btn btn-default" title="Print" data-ng-show="showReport" onclick="window.print()" style="width:40px;color:black; vertical-align: baseline;position: relative;top: 0.45em;" ><i class=" fa fa-print fa-lg"></i></button>       
                            <button id="search" data-ng-click="search()" class="btn btn-success" type="submit" title="Search for drugs"><i class="fa fa-search fa-lg" style="font-size:14px"> SEARCH</i></button>
                            <button id="clear"  onclick="history.go(0)" class="btn btn-info" title="Reset screen"><i class="fa fa-eraser fa-lg" style="font-size:14px"> CLEAR</i></button>                      
                        </div>
                    </div>
                   
                </div>
                <div id="therapeutic_group" class="col-sm-6">
                    <div id="group-class-selectionboxes"  class="row"  style="margin-bottom:0;padding-bottom:0;"> 
                        <div class="col-xs-6" >                         
                            <div class="list-group-item active"><strong>THERAPEUTIC  &nbsp;GROUP</strong></div>
                        
                            <select id="drugGroup" multiple="multiple"  class="" style="width:100%" 
                                    data-ng-model="groupCode"  data-ng-change="getTherapeuticClasses(groupCode)" >
                                <option  id="grouplist"  value="{{tg.tg_code}}"  
                                        data-ng-repeat="tg in pharmaGroups  | orderBy: 'tg_desc'">{{tg.tg_desc}}  
                                </option>
                            </select>
                      
                        </div>
                        <div class="col-xs-6">
                            <div class="list-group-item active"><strong>THERAPEUTIC &nbsp;CLASS</strong></div>
                            <select id="drugClass"  multiple="multiple"   class="" style="width:100%"  
                                    data-ng-model="classCode"  data-ng-change="saveTherapeuticClass(classCode)">
                                <option id="classlist" class=""  value="{{tc.tc_code}}"
                                        data-ng-repeat="tc in pharmaClasses | orderBy: 'tc_desc'" >{{tc.tc_desc}}</option>
                            </select>
                        </div>
                    </div>
                    <form id="frmSearch"   role="form" class="form-horizontal Search" >
                        <div id="input_box" >
                            <div class="rowSearchType rowSearch_PRODUCT"  data-ng-show="radioSelected == 'PRODUCT'"  style="margin-top:20px" >
                                <label class="control-label" for="PRODUCT" >PRODUCT NAME :</label>
                                <input type="text" data-ng-model="product"  id="PRODUCT" class="form-control" placeholder="(Enter partial or complete generic or brand name)" style="width:80%" />  
                            </div>
                            <div class="row rowSearchType rowSearch_PHARMCLASS"  data-ng-show="radioSelected == 'PHARMCLASS'" style="margin-top:35px">
                                <label class="control-label" for="PHARMCLASS">THERAPEUTIC GROUP OR CLASS NAME :</label>
                                <div class="ui-widget">
                                    <input id="therapeutic-groupclass"  type="text"  class="form-control"   style="font-family:Arial;width:80%"
                                        data-ng-model="typedGroupClass" placeholder="(Enter partial name containing group or class name)" />
                                </div>
                            </div>
                            <div class="rowSearchType rowSearch_MANUFACTURER" data-ng-show="radioSelected == 'MANUFACTURER'"  style="margin-top:35px">
                                <label class="control-label">MANUFACTURER NAME :</label>
                                <input type="text" data-ng-model="manufacturer" class="form-control" placeholder="(Enter partial or complete manufacturer.)"  style="width:80%"/>               
                            </div>
                            <div class="row rowSearchType rowSearch_NDC" data-ng-show="radioSelected == 'NDC'" style="margin-top:35px">
                                <label class="control-label" for="NDC">NATIONAL DRUG CODE (NDC):</label>
                                <input type="text" data-ng-model="ndc" class="form-control" id="NDC" placeholder="(Enter partial or complete NDC.)"   style="width:80%" />
                            </div>
                            <div class="row rowSearchType rowSearch_GPI"  data-ng-show="radioSelected == 'GPI'" style="margin-top:35px">
                                <label class="control-label" for="GPI">GENERIC PRODUCT IDENTIFIER (GPI) :</label>
                                <input type="text" data-ng-model="gpi" class="form-control" id="GPI" placeholder="(Enter partial or complete GPI Code)"   style="width:80%"/>
                            </div>
                            <div style="font-size:14px;margin:3px" data-ng-show="radioSelected == 'PRODUCT'">
                                To search for names that start with the input, add a single quote in front of the name.<br /><br />
                                <span style="font-size:12px">e.g.) &nbsp <strong> CAFFEINE </strong> lists all products with generic or brand names that contain CAFFEINE anywhere in the name.<br />
                                e.g.) &nbsp <strong> 'CAFFEINE </strong> lists products with names that start with CAFFEINE.</span>
                            </div>
                            <div style="font-size:14px;margin:3px" data-ng-show="radioSelected == 'MANUFACTURER'">
                                To search for names that start with the input, add a single quote in front of the name.
                            </div>
                            <div style="font-size:14px;margin-left:-10px" data-ng-show="radioSelected == 'PHARMCLASS'">
                                Enter a partial name to see the list of selections.<br />Names that appear on the list that are in capital letters are Therapeutic Group names.
                            </div>
                        </div>
                    </form>
                </div>
                <div  class="col-sm-2">
                    <div  style="margin-right:15px">
                       <label style="margin-top:-10px" class="checkbox-inline"><input  type="checkbox"  data-ng-model="showColumnList" style="width:10px" /><span id="columnDisplayLabel">Column List</span></label>
                       <label style="margin-top:-10px" class="checkbox-inline" data-ng-show="showColumnList"><input id="columnDisplayChk" type="checkbox" data-ng-click="selectAllColumns()"   style="width:10px"/><span id="Span1">Select all</span></label><br />
                        <div class="list-group-item active" style="background-color:gray;" id="selectcolumns" data-ng-show="showColumnList"><strong>&nbsp; Select Columns</strong></div>
                        <form id="columns"   data-ng-show="showColumnList">
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.GPI" />GPI</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Therapeutic_Group" />Therapeutic Group</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Name" />Name</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Generic_Name" />Generic Name</label><br />
                             <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.NDC" />NDC</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Source" />Source</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Rx_OTC" />Rx OTC</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Strength" />Strength</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Manufacturer" />Manufacturer</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Repackage_Code" />Repackaged</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Pkg_Size" />Pkg Size</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Pkg_Qty" />Pkg Qty</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Pkg_Price" />Pkg AWP</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Pkg_Count" />Pkg Count</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Unit_Price" />Unit AWP</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Price_Effective_Date" />Price Effective Date</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.GEAP" />GEAP</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Older_Unit_Price" />Previous Unit AWP</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Older_Pkg_Price" />Previous Pkg AWP</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Older_Price_Effective_Date"   />Previous Price Effective Date</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.DEA" />DEA</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Dosage_Form" />Dosage Form</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Therapeutic_Class" />Therapeutic Class</label><br />
                            <label class="checkbox-inline"><input type="checkbox" data-ng-model="showColumns.Name_Type"  />Name Type</label><br />
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="info" class="row-fluid no-print">
        <div data-ng-show="showReport"  style="color:#b30000;margin:10px 0;height:14px">
            <i class="fa fa-search col-xs-2" style="color:red;"> Double-click GPI# to search</i> 
            <i class="fa fa-info-circle col-xs-2"> Click NDC# for INGREDIENTS</i>
            <span style="text-align:right" class="col-xs-3"><i id="I3" class="fa fa-arrow-right" style="color:#3b5998;"> To MOVE a column, drag above the column name</i></span>
            <span style="text-align:right" class="col-xs-3"><i class="fa fa-usd"> For PRICE HISTORY, click on the PRICE.</i></span>
            <span style="text-align:right" class="col-xs-2"><i id="I1" class="fa fa-sort" style="color:#3b5998;"> To SORT, click the column name</i></span>
        </div>
        <div>
            <button class="btn btn-warning btn-lg" id="display_message" data-ng-hide="displayMessage == ''"  data-ng-bind-html="displayMessage" >{{displayMessage}}</button>
            <button class="btn btn-danger btn-lg"  id ="error_message" data-ng-hide="errorMessage == ''"    data-ng-bind-html="errorMessage" >{{errorMessage}}</button>
        </div>
    </div> 
    <div id="pharma_report"  data-ng-show="showReport" >   
        <table id="pharma_reportTable" class="table table-hover table-condensed" style="border-bottom:none;overflow:scroll">
            <thead>
                <tr id="draggable_header" >
                    <th id="Th1" data-ng-show="showColumns.GPI"  class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('gpi')" title="Generic Product Identifier: 14-character hierarchical classification system that identifies drugs from their primary therapeutic use down to the unique interchangeable product regardless of manufacturer or package size.">GPI  <span class="sortorder"  data-ng-show="predicate === 'gpi'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th2" data-ng-show="showColumns.Therapeutic_Group" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('therapeutic_group')" title="Drug group - classifies general drug products">Therapeutic Group  <span class="sortorder"  data-ng-show="predicate === 'therapeutic_group'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th3" data-ng-show="showColumns.Name" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('name')" title="Drug Product Name">Name <span class="sortorder"  data-ng-show="predicate === 'name'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th4" data-ng-show="showColumns.Generic_Name" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('generic_name')" title="Generic Product Name">Generic Name  <span class="sortorder"  data-ng-show="predicate === 'generic_name'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th5" data-ng-show="showColumns.NDC"  class="draggable-row" >
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('ndc')" title="National Drug Code - Unique identifier used in the United States for drugs intended for human use.">NDC  <span class="sortorder"  data-ng-show="predicate === 'ndc'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th6" data-ng-show="showColumns.Source" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('source')" title="Drug available from one labeler (single-source) or multiple labelers(multiple-source)">Source  <span class="sortorder"  data-ng-show="predicate === 'source'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th7" data-ng-show="showColumns.Rx_OTC" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('rx_otc')" title="Indicates federal prescription (Rx) or over-the-counter (OTC) status">RX OTC  <span class="sortorder"  data-ng-show="predicate === 'rx_otc'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th8" data-ng-show="showColumns.Strength" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('strength')" title="Dosage strength as provided by the manufacturer">Strength  <span class="sortorder"  data-ng-show="predicate === 'strength'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th9" data-ng-show="showColumns.Manufacturer" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('manufacturer')" title="Drug manufacturer"> Manufacturer  <span class="sortorder"  data-ng-show="predicate === 'manufacturer'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th10" data-ng-show="showColumns.Repackage_Code" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('repackage_code')" title="'X' indicates the drug has been repackaged"> Repackage Code  <span class="sortorder"  data-ng-show="predicate === 'repackage_code'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th11" data-ng-show="showColumns.Pkg_Size" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('pkg_size')" title="Total size of the package in volume or number of units contained">Pkg Size  <span class="sortorder"  data-ng-show="predicate === 'pkg_size'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th12" data-ng-show="showColumns.Pkg_Qty" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('pkg_qty')" title="Total Package Quantity (Pkg Size X Pkg Count)">Pkg Qty  <span class="sortorder"  data-ng-show="predicate === 'pkg_qty'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th3" data-ng-show="showColumns.Pkg_Price" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('pkg_price')" title="Average Wholesale Package Price">Pkg AWP  <span class="sortorder"  data-ng-show="predicate === 'pkg_price'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th14" data-ng-show="showColumns.Pkg_Count" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('pkg_count')" title="Number of individual containers or units per package">Pkg Count  <span class="sortorder"  data-ng-show="predicate === 'pkg_count'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th15" data-ng-show="showColumns.Unit_Price" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('unit_price')" title="Average Wholesale Unit Price">Unit AWP  <span class="sortorder"  data-ng-show="predicate === 'unit_price'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th16" data-ng-show="showColumns.Price_Effective_Date" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('price_effective_date')" title="Effective date of the price">Price Effective  <span class="sortorder"  data-ng-show="predicate === 'price_effective_date'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th17" data-ng-show="showColumns.GEAP" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('geap')" title="Generic Equivalent Average Price - Average of AWPs for all multi-source drug products.">GEAP  <span class="sortorder"  data-ng-show="predicate === 'geap'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th18" data-ng-show="showColumns.Older_Unit_Price" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('older_unit_price')">Previous Unit AWP  <span class="sortorder"  data-ng-show="predicate === 'older_unit_price'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th19" data-ng-show="showColumns.Older_Pkg_Price" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('older_pkg_price')">Previous Pkg AWP  <span class="sortorder"  data-ng-show="predicate === 'older_pkg_price'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th20" data-ng-show="showColumns.Older_Price_Effective_Date" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('older_price_effective_date')">Previous Price Effective  <span class="sortorder"  data-ng-show="predicate === 'older_price_effective_date'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th21" data-ng-show="showColumns.DEA" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('dea')" title="Drug Enforcement Administration Class Code identifies federally controlled substances classified by DEA">DEA  <span class="sortorder"  data-ng-show="predicate === 'dea'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th22" data-ng-show="showColumns.Dosage_Form" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('dosage_form')" title="Tablet, Powder, Solution, or Injection">Dosage Form  <span class="sortorder"  data-ng-show="predicate === 'dosage_form'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th23" data-ng-show="showColumns.Therapeutic_Class" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('therapeutic_class')" title="Drug class - identifies specific therapeutic drug classes">Therapeutic Class  <span class="sortorder"  data-ng-show="predicate === 'therapeutic_class'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                    <th id="Th24" data-ng-show="showColumns.Name_Type" class="draggable-row">
                        <div class="dragtable-handle"></div>
                        <div class="header_title" data-ng-click="sortData('name_type')" title="Trademarked, Branded Generic, or Generic Name">Name Type  <span class="sortorder"  data-ng-show="predicate === 'name_type'"  data-ng-class="{reverse:reverse}"></span></div>
                    </th>
                </tr>
            </thead>
            <tbody id="pharma_reportData" data-ng-repeat="p in pharmaResult | orderBy : predicate : reverse" data-ng-switch on="showPriceHistory[$index] || showIngredient[$index]" >
              <tr> 
                <td id="d2" data-ng-show="showColumns.GPI" class="clickableColumn" data-ng-dblclick="filterByGPI(p.gpi)"  title="Double-click to search by the selected GPI.">&#8203;{{p.gpi}}</td>
                <td id="d2" data-ng-show="showColumns.Therapeutic_Group">{{p.therapeutic_group}}</td>
                <td id="d3" data-ng-show="showColumns.Name">{{p.name}}</td>
                <td id="d4" data-ng-show="showColumns.Generic_Name"  data-ng-click="displayGenericNames ()"  title="Click to see unique names."><a href="#popup1">{{p.generic_name}}</a></td>
                <td id="d5" data-ng-show="showColumns.NDC" class="clickableColumn"  data-ng-click="displayIngredient ($index, p.ndc)"  title="Click to see ingredients.">&#8203;{{p.ndc}}</td>
                <td id="d6" data-ng-show="showColumns.Source">{{p.source}}</td>
                <td id="d7" data-ng-show="showColumns.Rx_OTC" class="center">{{p.rx_otc}}</td>
                <td id="d8" data-ng-show="showColumns.Strength" class="number">{{p.strength}}</td>
                <td id="d9" data-ng-show="showColumns.Manufacturer">{{p.manufacturer}}</td>
                <td id="d10" data-ng-show="showColumns.Repackage_Code" class="center" title="'X' indicates the drug has been repackaged">{{p.repackage_code}}</td>
                <td id="d11" data-ng-show="showColumns.Pkg_Size"  class="number">{{p.pkg_size}}</td>
                <td id="d12" data-ng-show="showColumns.Pkg_Qty"  class="number">{{p.pkg_qty}}</td>
                <td id="d13" data-ng-show="showColumns.Pkg_Price"  class="number clickableColumn" data-ng-click="displayPriceHistory ($index, p.ndc)" title="Click to see price history.">{{p.pkg_price | DisplayNullCurrency}}</td>
                <td id="d14" data-ng-show="showColumns.Pkg_Count"  class="number">{{p.pkg_count}}</td>
                <td id="d15" data-ng-show="showColumns.Unit_Price" class="number clickableColumn" data-ng-click="displayPriceHistory ($index, p.ndc)" title="Click to see price history.">{{p.unit_price | DisplayNullCurrency}}</td>
                <td id="d16" data-ng-show="showColumns.Price_Effective_Date" class="center">{{p.price_effective_date | date : 'MM/dd/yyyy' | DisplayNullDate }}</td>
                <td id="d17" data-ng-show="showColumns.GEAP" class="number">{{p.geap | DisplayNullCurrency}}</td>
                <td id="d18" data-ng-show="showColumns.Older_Unit_Price" class="number clickableColumn"  data-ng-click="displayPriceHistory ($index, p.ndc)" title="Click to see price history.">{{p.older_unit_price | DisplayNullCurrency}}</td>
                <td id="d19" data-ng-show="showColumns.Older_Pkg_Price"  class="number clickableColumn"  data-ng-click="displayPriceHistory ($index, p.ndc)" title="Click to see price history.">{{p.older_pkg_price | DisplayNullCurrency}}</td>
                <td id="d20" data-ng-show="showColumns.Older_Price_Effective_Date" class="center">{{p.older_price_effective_date | date : 'MM/dd/yyyy' | DisplayNullDate }}</td>
                <td id="d21" data-ng-show="showColumns.DEA" class="center">{{p.dea}}</td>
                <td id="d22" data-ng-show="showColumns.Dosage_Form" class="center">{{p.dosage_form}}</td>
                <td id="d23" data-ng-show="showColumns.Therapeutic_Class">{{p.therapeutic_class}}</td>
                <td id="d24" data-ng-show="showColumns.Name_Type">{{p.name_type}}</td>
              </tr>
              <tr  data-ng-switch-when="true"  style="border:none;">
                 <td colspan="12" style="border:none;margin:0;padding:0" data-ng-show="showPriceHistory[$index]" data-ng-hide="showIngredient[$index] || showGenericNames[$index]">
                     <div class="span12" style="border:none">
                        <div class="pull-right" style="border:none">
                            <table class="table table-condensed" style="margin:-1px 0;padding:0;border-bottom:none">
                                <thead class="levelTwo">
                                    <tr>
                                        <th>NDC</th>
                                        <th>Pkg Price</th>
                                        <th>Unit Price</th>
                                        <th>Effective Date</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr data-ng-repeat="ph in pharmaPriceHistory">
                                        <td>{{ph.ndc}}</td>
                                        <td class="number">{{ph.pkg_awp | currency}}</td>
                                        <td class="number">{{ph.unit_awp | currency}}</td>
                                        <td class="center">{{ph.awp_effective_date  | date : 'MM/dd/yyyy'}}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                     </div>
                 </td>
                 <td  colspan="12" style="border:none;margin:0;padding:0" data-ng-show="showIngredient[$index]" data-ng-hide="showPriceHistory[$index] || showGenericNames[$index]">
                     <div class="span12" style="border:none">
                        <div class="pull-left" style="border:none">
                            <table class="table table-condensed" style="margin:-1px 0;padding:0;border-bottom:none">
                                <thead class="levelTwo">
                                    <tr>
                                        <th>NDC</th>
                                        <th>Ingredient Generic ID #</th>
                                        <th>Ingredient Strength</th>
                                        <th>Strength Unit Measure</th>
                                        <th>Generic Ingredient Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr data-ng-repeat="ph in pharmaIngredient">
                                        <td>{{ph.ndc}}</td>
                                        <td class="center">{{ph.ingredient_generic_id_number}}</td>
                                        <td class="center">{{ph.ingredient_strength}}</td>
                                        <td class="center">{{ph.strength_unit_measure}}</td>
                                        <td>{{ph.generic_ingredient_name}}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                     </div>
                 </td>
              </tr>
           </tbody>
        </table>
  </div>
   <div id="popup1" class="overlay" data-ng-show="showGenericNames">
	<div class="popup">
		<a class="close btn btn-default" href="#">&times;</a>
		<div class="content">
			<table >
                <thead style="background-color:lightgray;color:white">
                    <tr>
                        <th>GENERIC NAMES</th>
                    </tr>
                </thead>
                <tbody>                                    
                    <tr data-ng-repeat="ph in pharmaGenericNames">
                        <td style="color:white;border:none">{{ph.generic_name}}</td>
                    </tr>
                </tbody>
            </table>
		</div>
	</div>
</div> 
 <div id="scrollTop" style="position:fixed;right:0;bottom:0" title="Move to top"><a href="#top"><i class="fa fa-arrow-up fa-3x no-print" aria-hidden="true"></i></a></div>
</body>
</html>
